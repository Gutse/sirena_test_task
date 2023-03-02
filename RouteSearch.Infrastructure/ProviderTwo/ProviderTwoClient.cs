using Mapster;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Serializers.Json;
using RouteSearch.Configuration;
using RouteSearch.Contract;
using RouteSearch.Infrastructure.ProviderOne;
using System.Collections.Generic;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace RouteSearch.Infrastructure.ProviderTwo
{
    public class ProviderTwoClient: ISearchProviderClient
    {
        private readonly ProviderTwoOptions _options;
        private readonly IHttpClientFactory _httpClientFactory;
        public ProviderTwoClient(ProviderTwoOptions options, IHttpClientFactory httpClientFactory)
        {
            _options = options;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IReadOnlyCollection<Route>> Search(SearchDto request, CancellationToken cancellationToken)
        {
            var client = BuildClientForProviderTwo();
            var param = request.Adapt<ProviderOneSearchRequest>();
            var pr = new RestRequest($"{_options.ApiUrl}search", Method.Post).AddJsonBody(param);
            var response = await client.ExecuteAsync<ProviderOneSearchResponse>(pr, cancellationToken);
            if (!response.IsSuccessStatusCode || response.Data?.Routes == null)
            {
                throw new Exception("something went wrong");
            }

            return response.Data.Routes.Adapt<Route[]>();
        }

        private RestClient BuildClientForProviderTwo()
        {
            var client = new RestClient(_httpClientFactory.CreateClient("fake2"));
            client.UseSerializer(() => new SystemTextJsonSerializer(new JsonSerializerOptions()));
            return client;
        }

        public async Task<bool> IsAvailable(CancellationToken cancellationToken)
        {
            var client = BuildClientForProviderTwo();

            var pr = new RestRequest($"{_options.ApiUrl}ping", Method.Get);
            var response = await client.ExecuteAsync<ProviderOneSearchResponse>(pr, cancellationToken);
            return response.IsSuccessStatusCode;
        }
    }
}