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
    public class PingPongPolicyTests : KeepAlivePolicyTestBase
    {

        public PingPongPolicyTests(ITestOutputHelper output) : base(output)
        {
            ConfigureKeepAlivePolicy(new PingPongPolicy());
        }

        [Fact]
        public async Task ApplyStrategy_should_return_if_server_does_not_respond_a_ping_with_a_pong_after_sometime()
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
            await FlushData();
            Client.Streamer.Received().PublishInboundMessageOnStream(Arg.Any<string>());
        }

    }
}
