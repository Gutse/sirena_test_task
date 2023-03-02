using System;

namespace RouteSearch.Contract
{
    public class SearchDto
    {
        public SearchDto(string origin, string destination, DateTime originDateTime)
        {
            Origin = origin;
            Destination = destination;
            OriginDateTime = originDateTime;
        }

        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime OriginDateTime { get; set; }
        public SearchFilters? Filters { get; set; }
    }
}