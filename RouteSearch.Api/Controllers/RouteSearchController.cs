using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RouteSearch.Api.Contract;
using RouteSearch.Contract;
using RouteSearch.Core.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RouteSearch.Api.Controllers
{
    public class RouteSearchController: RouteSearchControllerBase
    {
        private readonly ISearchService _searchService;

        public RouteSearchController(IHttpContextAccessor httpContextAccessor, ISearchService searchService) : base(httpContextAccessor)
        {
            _searchService = searchService;
        }

        [HttpPost(nameof(Search))]
        [AllowAnonymous]
        public async Task<SearchResponse> Search([FromBody] SearchRequest request, CancellationToken cancellationToken)
        {
            var dto = request.Adapt<SearchDto>();
            var result = await _searchService.SearchAsync(dto, cancellationToken);
            return result.Adapt<SearchResponse>();
        }

        [HttpGet(nameof(IsAvailable))]
        [AllowAnonymous]
        public async Task IsAvailable(CancellationToken cancellationToken)
        {
            var result = await _searchService.IsAvailableAsync(cancellationToken);
            if (!result)
            {
                // by task we should return 500
                throw new Exception("no provider available");
            }
        }
    }
}