using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RouteSearch.Api.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class RouteSearchControllerBase : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RouteSearchControllerBase(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
