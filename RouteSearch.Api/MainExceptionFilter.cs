using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RouteSearch.Core.Errors;
using System.Linq;
using System.Net;

namespace RouteSearch.Api
{
    public class MainExceptionFilter : IActionFilter, IOrderedFilter
    {
        private readonly IHostEnvironment _environment;
        private readonly ILogger<MainExceptionFilter> _logger;

        public MainExceptionFilter(IHostEnvironment environment, ILogger<MainExceptionFilter> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        public int Order { get; set; } = int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            switch (context.Exception)
            {
                case LogicException exception:
                    var details = new ProblemDetails
                    {
                        Status = (int)HttpStatusCode.BadRequest,
                        Title = exception.Error?.Description ?? exception.Message,
                        Extensions = {{"Error", exception.Error}}
                    };

                    if (!_environment.IsProduction())
                    {
                        details.Extensions.Add("Exception", exception.ToString());
                    }

                    context.Result = new BadRequestObjectResult(details);
                    context.ExceptionHandled = true;
                    _logger.LogError(exception, "Logic exception have been thrown");
                    break;

                case ValidationException exception:
                    context.Result = new BadRequestObjectResult(new
                    {
                        Description = exception.Message,
                        Errors = exception.Errors.ToList()
                    });

                    context.ExceptionHandled = true;
                    break;

                case { } exception:
                    _logger.LogError(exception, "Unhandled exception have been thrown");
                    break;
            }
        }
    }
}
