using RouteSearch.Contract;
using System;
using System.ComponentModel.DataAnnotations;

namespace RouteSearch.Api.Contract
{
    public class SearchRequest
    {
        /// <summary>
        /// Mandatory
        /// Start point of route, e.g. Moscow 
        /// </summary>
        [Required]
        [StringLength(1024, MinimumLength = 3)]
        public string? Origin { get; set; }

        /// <summary>
        /// Mandatory
        /// End point of route, e.g. Sochi
        /// </summary>
        [Required]
        [StringLength(1024, MinimumLength = 3)]
        public string? Destination { get; set; }

        /// <summary>
        /// Mandatory
        /// Start date of route
        /// </summary>
        [Required]
        public DateTime OriginDateTime { get; set; }

        /// <summary>
        /// Optional
        /// </summary>
        public SearchFilters? Filters { get; set; }
    }
}