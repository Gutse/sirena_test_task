using RouteSearch.Contract;
using RouteSearch.Core.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace RouteSearch.Infrastructure
{
    public class CachedSearchService : ISearchService
    {
        private readonly SearchService _searchService;
        private readonly IRoutesCache _routesCache;

        public CachedSearchService(SearchService searchService, IRoutesCache routesCache)
        {
            _searchService = searchService;
            _routesCache = routesCache;
        }

        public async Task<SearchResultDto> SearchAsync(SearchDto request, CancellationToken cancellationToken)
        {
            if (request.Filters?.OnlyCached ?? false)
            {
                return await _routesCache.SearchInCache(request, cancellationToken);
            }

            var result = await _searchService.SearchAsync(request, cancellationToken);
            await _routesCache.CacheResults(request, result);
            return result;
        }

        public Task<bool> IsAvailableAsync(CancellationToken cancellationToken)
        {
            return _searchService.IsAvailableAsync(cancellationToken);
        }
    }
}