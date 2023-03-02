using Microsoft.Extensions.Caching.Memory;
using RouteSearch.Contract;
using RouteSearch.Core;
using RouteSearch.Core.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
#pragma warning disable CS1030

namespace RouteSearch.Infrastructure
{
#warning !!!
    // I think it reasonable to store routes in database (sql or nosql) and just query it for matching routes
    // but I will not going to waste my time for this just for test task. or we can discuss it on interview

    public class InMemRoutesCache : IRoutesCache
    {
        private readonly ConcurrentDictionary<Guid, Route> _cache;
        public InMemRoutesCache(IMemoryCache memoryCache)
        {
            _cache = new ConcurrentDictionary<Guid, Route>();
        }

        public Task<SearchResultDto> SearchInCache(SearchDto request, CancellationToken cancellationToken)
        {
            var cached = _cache.Select(c => c.Value).ToArray();
            var result = RoutesAggregator.AggregateRoutes(cached);
            return Task.FromResult(result);
        }

        public Task CacheResults(SearchDto request, SearchResultDto result)
        {
            if (result.Routes == null || result.Routes.Length == 0)
            {
                return Task.CompletedTask;
            }

            foreach (var route in result.Routes)
            {
                if (_cache.TryGetValue(route.Id, out var cached))
                {
                    if (cached.TimeLimit < DateTime.UtcNow)
                    {
                        _cache.TryRemove(cached.Id, out var _);
                    }
                    else
                    {
                        _cache.TryUpdate(route.Id, route, cached);
                    }
                }
                else
                {
                    if (route.TimeLimit >= DateTime.UtcNow)
                    {
                        _cache.TryAdd(route.Id, route);
                    }
                }
            }

            return Task.CompletedTask;
        }

       
    }
}