using RouteSearch.Contract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RouteSearch.Core
{
    public static class RoutesAggregator
    {
        public static SearchResultDto AggregateRoutes(IReadOnlyCollection<Route>? routes)
        {
            if (routes == null || routes.Count == 0)
            {
                Errors.Errors.ThrowError(Errors.Errors.NoResults);
            }

            var result = new SearchResultDto();
            var resultRoutes = new Dictionary<Guid, Route>();
            result.MaxPrice = decimal.MinValue;
            result.MaxMinutesRoute = int.MinValue;
            result.MinMinutesRoute = int.MaxValue;
            result.MinPrice = decimal.MaxValue;

            foreach (var route in routes)
            {
                if (resultRoutes.ContainsKey(route.Id))
                {
                    continue;
                }

                resultRoutes.Add(route.Id, route);

                var duration = (int)(route.DestinationDateTime - route.OriginDateTime).TotalMinutes;

                if (result.MaxMinutesRoute <= duration)
                {
                    result.MaxMinutesRoute = duration;
                }

                if (result.MaxPrice <= route.Price)
                {
                    result.MaxPrice = route.Price;
                }

                if (result.MinMinutesRoute >= duration)
                {
                    result.MinMinutesRoute = duration;
                }

                if (result.MinPrice >= route.Price)
                {
                    result.MinPrice = route.Price;
                }
            }

            result.Routes = resultRoutes.Values.ToArray();
            return result;
        }
    }
}