using RouteSearch.Contract;
using System.Threading;
using System.Threading.Tasks;

namespace RouteSearch.Core.Abstractions;

public interface ISearchService
{
    Task<SearchResultDto> SearchAsync(SearchDto request, CancellationToken cancellationToken);
    Task<bool> IsAvailableAsync(CancellationToken cancellationToken);
}