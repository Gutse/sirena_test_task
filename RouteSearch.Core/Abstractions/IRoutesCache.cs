using RouteSearch.Contract;
using System.Threading;
using System.Threading.Tasks;

namespace RouteSearch.Core.Abstractions
{
    public interface IRoutesCache
    {
        Task<SearchResultDto> SearchInCache(SearchDto request, CancellationToken cancellationToken);
        Task CacheResults(SearchDto request, SearchResultDto result);
    }
}