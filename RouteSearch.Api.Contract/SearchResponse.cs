using RouteSearch.Contract;
using System;

namespace RouteSearch.Api.Contract
{
    public class SearchResponse
    {
        /// <summary>
        /// Mandatory
        /// Array of routes
        /// </summary>
        public Route[]? Routes { get; set; }
        
        /// <summary>
        /// Mandatory
        /// The cheapest route
        /// </summary>
        public decimal MinPrice { get; set; }

        /// <summary>
        /// Mandatory
        /// Most expensive route
        /// </summary>
        public decimal MaxPrice { get; set; }

        /// <summary>
        /// Mandatory
        /// The fastest route
        /// </summary>
        public int MinMinutesRoute { get; set; }

        /// <summary>
        /// Mandatory
        /// The longest route
        /// </summary>
        public int MaxMinutesRoute { get; set; }
    }
}