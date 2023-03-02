using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RichardSzalay.MockHttp;
using RouteSearch.Api.Contract;
using RouteSearch.Api.Validation;
using RouteSearch.Configuration;
using RouteSearch.Core.Abstractions;
using RouteSearch.Infrastructure;
using RouteSearch.Infrastructure.ProviderOne;
using RouteSearch.Infrastructure.ProviderTwo;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace RouteSearch.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _environment;

        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            TypeAdapterConfig.GlobalSettings.Default.PreserveReference(true);
            //TypeAdapterConfig.GlobalSettings.RequireDestinationMemberSource = true;
            TypeAdapterConfig.GlobalSettings.Scan(typeof(ApiContractsMapping).Assembly);
            TypeAdapterConfig.GlobalSettings.Scan(typeof(ProviderOneMappings).Assembly);
            TypeAdapterConfig.GlobalSettings.Compile();


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RouteSearch.Api", Version = "v1" });
                c.CustomSchemaIds(type =>
                {
                    var name = type.Name;
                    if (type.GenericTypeArguments.Length > 0)
                    {
                        name = type.Name.Replace("`1", "");
                        name = string.Join("", type.GenericTypeArguments.Select(x => x.Name)) + name;
                    }
                    return name;
                });
                c.DescribeAllParametersInCamelCase();
            });

            services.AddEndpointsApiExplorer();
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining(typeof(SearchRequestValidator));
            services.AddMemoryCache();
            services.AddHttpContextAccessor();

            services.AddCors(options =>
            {
                options.AddPolicy("default", builder =>
                    builder.WithOrigins(_configuration.GetSection("Origins").Get<string[]>())
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .WithMethods("GET", "POST", "PUT", "OPTIONS", "DELETE"));
            });
            services.AddControllers(options =>
            {
                options.Filters.Add<MainExceptionFilter>();
                //options.Filters.Add(new AuthorizeFilter(defaultPolicy));
            });

            var mockForProviderOne = new MockHttpMessageHandler();
            var mockForProviderTwo = new MockHttpMessageHandler();
            mockForProviderOne.When(HttpMethod.Get, "*").Respond(() =>
            {
                return ReturnPing();
            });

            mockForProviderTwo.When(HttpMethod.Get, "*").Respond(() =>
            {
                return ReturnPing();
            });

            mockForProviderOne.When(HttpMethod.Post, "*").Respond(() =>
            {
                var response = new ProviderOneSearchResponse
                {
                    Routes = new[] { new ProviderOneRoute { Price = 100, TimeLimit = DateTime.UtcNow.AddDays(1) } }
                };
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonSerializer.Serialize(response))
                });
            });

            mockForProviderTwo.When(HttpMethod.Post, "*").Respond(() =>
            {
                var response = new ProviderTwoSearchResponse()
                {
                    Routes = new[] { new ProviderTwoRoute() { Price = 100, TimeLimit = DateTime.UtcNow.AddDays(1) } }
                };
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonSerializer.Serialize(response))
                });
            });


            services.AddHttpClient("fake1").ConfigureHttpMessageHandlerBuilder(builder => builder.PrimaryHandler = mockForProviderOne);
            services.AddHttpClient("fake2").ConfigureHttpMessageHandlerBuilder(builder => builder.PrimaryHandler = mockForProviderTwo);

            services.AddConfigurationItems(_configuration);
            services.AddHealthChecks();

            services.AddScoped<ISearchService, CachedSearchService>();
            services.AddScoped<SearchService>();
            services.AddScoped<ISearchProviderClient, ProviderOneClient>();
            services.AddScoped<ISearchProviderClient, ProviderTwoClient>();
            services.AddSingleton<IRoutesCache, InMemRoutesCache>();
        }

        private static Task<HttpResponseMessage> ReturnPing()
        {
            var rnd = new Random();
            if (rnd.Next(1, 100) > 90)
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider container, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.DocumentTitle = "RouteSearch.Api Documentation";
                    c.DocExpansion(DocExpansion.List);
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //todo or it should be IsAlive?
                endpoints.MapHealthChecks("/hc");
            });
        }
    }
}
