// Tests for ConfigurationServiceCollectionExtensions built with xUnit.
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RouteSearch.Configuration;
using Xunit;

namespace RouteSearch.Configuration.Tests
{
    public class ConfigurationServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddConfigurationItems_WithValidConfiguration_RegistersProviderOptionsAsSingletons()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["ProviderOneOptions:ApiUrl"] = "https://provider-one.example",
                    ["ProviderTwoOptions:ApiUrl"] = "https://provider-two.example"
                })
                .Build();

            // Act
            var returned = services.AddConfigurationItems(configuration);

            // Assert
            Assert.Same(services, returned);

            using var provider = services.BuildServiceProvider();

            var providerOne = provider.GetRequiredService<ProviderOneOptions>();
            var providerTwo = provider.GetRequiredService<ProviderTwoOptions>();

            Assert.Equal("https://provider-one.example", providerOne.ApiUrl);
            Assert.Equal("https://provider-two.example", providerTwo.ApiUrl);

            var descriptor = services.Single(sd => sd.ServiceType == typeof(ProviderOneOptions));
            Assert.Equal(ServiceLifetime.Singleton, descriptor.Lifetime);
            Assert.Same(providerOne, descriptor.ImplementationInstance);
        }

        [Fact]
        public void AddConfigurationItems_WhenValidationFails_ThrowsArgumentNullException()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["ProviderOneOptions:ApiUrl"] = "",
                    ["ProviderTwoOptions:ApiUrl"] = "https://provider-two.example"
                })
                .Build();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => services.AddConfigurationItems(configuration));
        }

        [Fact]
        public void AddConfigurationItems_WhenCalledMultipleTimes_AddsAdditionalDescriptors()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["ProviderOneOptions:ApiUrl"] = "https://provider-one.example",
                    ["ProviderTwoOptions:ApiUrl"] = "https://provider-two.example"
                })
                .Build();

            // Act
            services.AddConfigurationItems(configuration);
            services.AddConfigurationItems(configuration);

            // Assert
            var providerOneDescriptors = services.Where(sd => sd.ServiceType == typeof(ProviderOneOptions)).ToList();
            Assert.Equal(2, providerOneDescriptors.Count);
            Assert.All(providerOneDescriptors, descriptor => Assert.Equal(ServiceLifetime.Singleton, descriptor.Lifetime));
        }

        [Fact]
        public void AddConfigurationItems_WhenConfigurationIsNull_ThrowsNullReferenceException()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => services.AddConfigurationItems(null\!));
        }
    }
}