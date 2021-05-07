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
    public class PingPolicyTests : KeepAlivePolicyTestBase
    {

        public PingPolicyTests(ITestOutputHelper output) : base(output)
        {
            ConfigureKeepAlivePolicy(new PingPolicy());
        }

        [Fact]
        public async Task ApplyStrategy_should_send_ping_requests_to_the_server_every_x_time()
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
                await Task.Delay(10).ConfigureAwait(false);
            }

            Client.Streamer.Received().PublishInboundMessageOnStream(Arg.Any<string>());
        }

    }
}
