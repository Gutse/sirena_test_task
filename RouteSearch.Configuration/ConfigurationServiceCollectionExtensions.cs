using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace RouteSearch.Configuration
{
    public static class ConfigurationServiceCollectionExtensions
    {
        /// <summary>
        /// Registers IConfigurationItem implementations found in this assembly by binding configuration sections named after each type and adding the bound, validated instances as singletons to the service collection.
        /// </summary>
        /// <param name="collection">The IServiceCollection to register configuration items into.</param>
        /// <param name="configuration">The IConfiguration used to bind configuration sections to concrete types.</param>
        /// <returns>The original IServiceCollection with registered configuration item singletons.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the assembly containing ConfigurationServiceCollectionExtensions cannot be resolved.</exception>
        public static IServiceCollection AddConfigurationItems(this IServiceCollection collection, IConfiguration configuration)
        {
            var assembly = Assembly.GetAssembly(typeof(ConfigurationServiceCollectionExtensions));
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }
            
            var genericBaseConsumer = typeof(Action);

            foreach (var option in assembly.GetTypes())
            {
                var serviceInterface = option.GetInterfaces().FirstOrDefault(t => t == typeof(IConfigurationItem));
                if (serviceInterface != null)
                {
                    var value = configuration.GetSection(option.Name).Get(option);
                    if (value is IConfigurationItem item)
                    {
                        item.ValidateOrThrow();
                        collection.AddSingleton(value.GetType(), value);
                    }
                }
            }


            return collection;
        }
    }
}