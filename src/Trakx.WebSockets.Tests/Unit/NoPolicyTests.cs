using System;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using Trakx.WebSockets.KeepAlivePolicies;
using Trakx.WebSockets.Tests.Unit.DummyClient.Messages;
using Xunit;
using Xunit.Abstractions;

namespace Trakx.WebSockets.Tests.Unit
{
    public class NoPolicyTests : KeepAlivePolicyTestBase
    {

        public NoPolicyTests(ITestOutputHelper output) : base(output)
        {
            ConfigureKeepAlivePolicy(new NoPolicy());
        }

        [Fact]
        public async Task ApplyStrategy_should_not_do_anything_if_websocket_server_is_not_responding_for_a_long_time()
        {
            DateTimeProvider.UtcNow.Returns(DateTime.UtcNow);
            SimulateWebSocketResponse(new PriceChangedMessage
            {
                Symbol = "abc",
                Price = (decimal)1.99,
                Timestamp = DateTime.UtcNow,
                Type = PriceChangedMessage.TypeValue
            });
            await Client.Connect();
            while (!Client.Streamer.ReceivedCalls().Any())
            {
                await Task.Delay(10);
            }

            Client.Streamer.Received().PublishInboundMessageOnStream(Arg.Any<string>());
        }

    }
}
