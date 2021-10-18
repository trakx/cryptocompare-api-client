using Microsoft.Extensions.DependencyInjection;
using Trakx.CryptoCompare.ApiClient.WebSocket;
using Trakx.CryptoCompare.ApiClient.WebSocket.DTOs.Inbound;
using Trakx.Websocket.Interfaces;
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

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var socketStreamer = serviceProvider.GetRequiredService<IClientWebsocketRedirectRegistry<InboundMessageBase, CryptoCompareWebsocketSubscription>>();
        }
    }
}
