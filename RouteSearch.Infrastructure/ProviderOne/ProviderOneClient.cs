using Mapster;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Serializers.Json;
using RouteSearch.Configuration;
using RouteSearch.Contract;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace RouteSearch.Infrastructure.ProviderOne
{
    public class ProviderOneClient : ISearchProviderClient
    {
        private readonly ProviderOneOptions _options;
        private readonly IHttpClientFactory _clientFactory;

        public ProviderOneClient(ProviderOneOptions options, IHttpClientFactory clientFactory)
        {
            _options = options;
            _clientFactory = clientFactory;
        }

        public async Task<IReadOnlyCollection<Route>> Search(SearchDto request, CancellationToken cancellationToken)
        {
            var client = BuildClientForProviderOne();
            var param = request.Adapt<ProviderOneSearchRequest>();
            var pr = new RestRequest($"{_options.ApiUrl}search", Method.Post).AddJsonBody(param);
            var response = await client.ExecuteAsync<ProviderOneSearchResponse>(pr, cancellationToken);
            if (!response.IsSuccessStatusCode || response.Data?.Routes == null)
            {
                throw new Exception("something went wrong");
            }

            return response.Data.Routes.Adapt<Route[]>();
        }

        private RestClient BuildClientForProviderOne()
        {
            var client = new RestClient(_clientFactory.CreateClient("fake1"));
            client.UseSerializer(() => new SystemTextJsonSerializer(new JsonSerializerOptions()));
            return client;
        }

        public async Task<bool> IsAvailable(CancellationToken cancellationToken)
        {
            var client = BuildClientForProviderOne();
            
            var pr = new RestRequest($"{_options.ApiUrl}ping", Method.Get);
            var response = await client.ExecuteAsync<ProviderOneSearchResponse>(pr, cancellationToken);
            return response.IsSuccessStatusCode;
        }
    }
}