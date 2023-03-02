using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;

namespace RouteSearch.Api
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args).ConfigureLogger()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }

        public static IHostBuilder ConfigureLogger(this IHostBuilder hostBuilder)
        {
            hostBuilder.UseSerilog((context, _, cfg) =>
            {
                ConfigureSerilog(context, cfg);
            });
            return hostBuilder;
        }

        private static void ConfigureSerilog(HostBuilderContext hostBuilderContext, LoggerConfiguration loggerConfiguration)
        {
            loggerConfiguration
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails(new DestructuringOptionsBuilder())
                // .Enrich.WithThreadId()
                // .Enrich.WithThreadName()
                .ReadFrom.Configuration(hostBuilderContext.Configuration);
        }
    }
}