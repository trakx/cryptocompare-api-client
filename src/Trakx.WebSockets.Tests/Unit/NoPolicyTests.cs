using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using Trakx.WebSockets.KeepAlivePolicies;
using Trakx.WebSockets.Tests.Unit.DummyClient.Messages;
using Xunit;
using Xunit.Abstractions;

namespace Trakx.WebSockets.Tests.Unit
{
    public class NoPolicyTests : KeepAlivePolicyTestBase<NoPolicy>
    {

        public NoPolicyTests(ITestOutputHelper output) : base(output)
        {
            Policy = new NoPolicy();
            ConfigureKeepAlivePolicy(Policy);
        }

        [Fact]
        public async Task ApplyStrategy_should_not_do_anything_if_websocket_server_is_not_responding_for_a_long_time()
        {
            SimulateJsonResponse(new PriceChangedMessage
            {
                Symbol = "abc",
                Price = (decimal)1.99,
                Timestamp = DateTime.UtcNow,
                Type = PriceChangedMessage.TypeValue
            });
            Client.WebSocket.State.Returns(WebSocketState.Open);
            await Client.Connect();
            await FlushData();
            Client.WebSocket.State.Returns(WebSocketState.Closed);
            Client.Streamer.Received().PublishInboundMessageOnStream(Arg.Any<string>());
            await Client.WebSocket.DidNotReceive().RecycleConnectionAsync(Arg.Any<CancellationToken>());
            await Client.WebSocket.DidNotReceive().PingServer(Arg.Any<string>(), Arg.Any<CancellationToken>());
        }

    }
}
