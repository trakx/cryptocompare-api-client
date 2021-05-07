using Microsoft.Extensions.DependencyInjection;
using Trakx.CryptoCompare.ApiClient.WebSocket;
using Trakx.WebSockets;
using Trakx.WebSockets.KeepAlivePolicies;
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
            serviceCollection.AddCryptoCompareClient(configuration, new NoPolicy());

            var serviceProvider = serviceCollection.BuildServiceProvider();
            serviceProvider.GetRequiredService<IWebSocketAdapter>();
            serviceProvider.GetRequiredService<IKeepAlivePolicy>();
            serviceProvider.GetRequiredService<ICryptoCompareWebSocketStreamer>();
        }
    }
}
