using System;

namespace RouteSearch.Configuration
{
    public class ProviderTwoOptions: IConfigurationItem
    {
        public string? ApiUrl { get; set; }

        public void ValidateOrThrow()
        {
            if (string.IsNullOrWhiteSpace(ApiUrl))
            {
                throw new ArgumentNullException(nameof(ApiUrl));
            }
        }
    }
}