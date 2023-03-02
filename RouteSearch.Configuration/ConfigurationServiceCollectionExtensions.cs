using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace RouteSearch.Configuration
{
    public static class ConfigurationServiceCollectionExtensions
    {
        public static IServiceCollection AddConfigurationItems(this IServiceCollection collection, IConfiguration configuration)
        {
            var assembly = Assembly.GetAssembly(typeof(ConfigurationServiceCollectionExtensions));
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }
            //var genericBaseConsumer = typeof(BaseEventConsumer<,>);

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