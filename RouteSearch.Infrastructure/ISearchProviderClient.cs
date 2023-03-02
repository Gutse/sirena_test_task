using RouteSearch.Contract;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RouteSearch.Infrastructure
{
    public interface ISearchProviderClient
    {
        Task<IReadOnlyCollection<Route>> Search(SearchDto request, CancellationToken cancellationToken);
        Task<bool> IsAvailable(CancellationToken cancellationToken);
    }
}