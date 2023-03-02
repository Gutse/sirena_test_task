using RouteSearch.Contract;
using System;

namespace RouteSearch.Core
{
    public static class RouteMatcher
    {
        //todo theory unit tests
        public static bool RouteMatchesToSearch(Route route, SearchDto search)
        {
            var result =
                string.Equals(route.Destination, search.Destination, StringComparison.InvariantCultureIgnoreCase) &&
                string.Equals(route.Origin, search.Origin, StringComparison.InvariantCultureIgnoreCase);

            if (search.Filters != null)
            {
                result = result && route.Price <= search.Filters.MaxPrice;
            }
            //todo add other evaluations, can't waste time right now for test task

            return result;
        }
    }
}