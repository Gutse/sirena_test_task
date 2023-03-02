using Microsoft.Extensions.Logging;
using RouteSearch.Contract;
using RouteSearch.Core;
using RouteSearch.Core.Abstractions;
using RouteSearch.Core.Errors;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RouteSearch.Infrastructure
{
    public class SearchService : ISearchService
    {
        private readonly IEnumerable<ISearchProviderClient> _providers;
        private readonly ILogger<SearchService> _logger;


        public SearchService(IEnumerable<ISearchProviderClient> providers, ILogger<SearchService> logger)
        {
            _providers = providers;
            _logger = logger;
        }

        public async Task<SearchResultDto> SearchAsync(SearchDto request, CancellationToken cancellationToken)
        {
            if (!_providers.Any())
            {
                Errors.ThrowError(Errors.ProvidersNotConfigured);
            }


            var results = new ConcurrentBag<IReadOnlyCollection<Route>>();

            var tasks = new List<Task>();

            foreach (var provider in _providers)
            {
                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        var result = await provider.Search(request, cancellationToken);
                        results.Add(result);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "failed to search");
                        throw;
                    }
                }, cancellationToken));
            }

            var t = Task.WhenAll(tasks);
            await t;

            if (t.Exception != null)
            {
                //todo what should we do?
            }

            if (results.IsEmpty)
            {
                Errors.ThrowError(Errors.NoResults);
            }

            var allRoutes = new List<Route>();
            foreach (var route in results)
            {
                allRoutes.AddRange(route);
            }

            return RoutesAggregator.AggregateRoutes(allRoutes);
        }

        public async Task<bool> IsAvailableAsync(CancellationToken cancellationToken)
        {
            if (!_providers.Any())
            {
                return false;
            }

            var failed = 0;
            var tasks = new List<Task>();

            foreach (var provider in _providers)
            {
                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        var result = await provider.IsAvailable(cancellationToken);

                        if (!result)
                        {
                            Interlocked.Increment(ref failed);
                        }
                    }
                    catch
                    {
                        Interlocked.Increment(ref failed);
                    }
                }, cancellationToken));
            }
            await Task.WhenAll(tasks);

            // todo or should we return true if any of providers available?
            return failed == 0;
        }
    }
}