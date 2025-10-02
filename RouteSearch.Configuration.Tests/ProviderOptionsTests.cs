using System;
using RouteSearch.Configuration;
using Xunit;

namespace RouteSearch.Configuration.Tests
{
    // Testing library/framework: xUnit
    public class ProviderOptionsTests
    {
        [Fact]
        public void ProviderOneOptions_ValidateOrThrow_ShouldNotThrowWhenApiUrlPresent()
        {
            var options = new ProviderOneOptions { ApiUrl = "https://provider.one" };

            var exception = Record.Exception(() => options.ValidateOrThrow());

            Assert.Null(exception);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ProviderOneOptions_ValidateOrThrow_ShouldThrowWhenApiUrlMissing(string? apiUrl)
        {
            var options = new ProviderOneOptions { ApiUrl = apiUrl };

            var exception = Assert.Throws<ArgumentNullException>(() => options.ValidateOrThrow());

            Assert.Equal("ApiUrl", exception.ParamName);
        }

        [Fact]
        public void ProviderTwoOptions_ValidateOrThrow_ShouldNotThrowWhenApiUrlPresent()
        {
            var options = new ProviderTwoOptions { ApiUrl = "https://provider.two" };

            var exception = Record.Exception(() => options.ValidateOrThrow());

            Assert.Null(exception);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ProviderTwoOptions_ValidateOrThrow_ShouldThrowWhenApiUrlMissing(string? apiUrl)
        {
            var options = new ProviderTwoOptions { ApiUrl = apiUrl };

            var exception = Assert.Throws<ArgumentNullException>(() => options.ValidateOrThrow());

            Assert.Equal("ApiUrl", exception.ParamName);
        }
    }
}