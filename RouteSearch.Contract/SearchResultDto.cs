namespace RouteSearch.Contract
{
    public class SearchResultDto
    {
        public Route[]? Routes { get; set; }

        public decimal MinPrice { get; set; }

        public decimal MaxPrice { get; set; }

        public int MinMinutesRoute { get; set; }

        public int MaxMinutesRoute { get; set; }
    }
}