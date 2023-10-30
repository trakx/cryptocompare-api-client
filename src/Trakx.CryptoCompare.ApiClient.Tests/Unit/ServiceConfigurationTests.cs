using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Trakx.CryptoCompare.ApiClient.Tests.Unit
{
    public class ServiceConfigurationTests
    {
        [Fact]
        public void Services_should_be_built()
        {
            var configuration = new CryptoCompareApiConfiguration { ApiKey = "" };

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddCryptoCompareClient(configuration);
            serviceCollection.AddLogging();

            var action = () => _ = serviceCollection.BuildServiceProvider();

            action.Should().NotThrow();
        }
    }
}
