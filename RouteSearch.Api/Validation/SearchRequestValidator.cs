using FluentValidation;
using RouteSearch.Api.Contract;
using System;

namespace RouteSearch.Api.Validation
{
    public class SearchRequestValidator: AbstractValidator<SearchRequest>
    {
        public SearchRequestValidator()
        {
            RuleFor(r => r.Destination).NotNull().NotEmpty();
            RuleFor(r => r.Origin).NotNull().NotEmpty();
            RuleFor(r => r.OriginDateTime).NotNull().NotEmpty().GreaterThan(DateTime.UtcNow);
        }
    }
}