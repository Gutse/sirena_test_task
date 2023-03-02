using System;

namespace RouteSearch.Configuration
{
    public class ProviderOneOptions: IConfigurationItem
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