using System;
using System.Linq;
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
    public class PingPongPolicyTests : KeepAlivePolicyTestBase<PingPongPolicy>
    {

        public PingPongPolicyTests(ITestOutputHelper output)
            : base(output)
        {
            string pingMessage = "ping server";
            string expectedPongMessage = "pong server";
            var expectedPongInterval = TimeSpan.FromMinutes(3);
            Policy = new PingPongPolicy(pingMessage,
                expectedPongInterval, expectedPongMessage, DateTimeProvider,
                TestScheduler);
            ConfigureKeepAlivePolicy(Policy);
        }

        [Fact]
        public async Task ApplyStrategy_should_not_reconnect_if_ping_pong_logic_intervals_are_respected()
        {
            DateTimeProvider.UtcNow.Returns(DateTime.UtcNow);
            SimulateWebSocketResponse(new HeartBeatMessage
            {
                Timestamp = DateTime.UtcNow,
                Type = HeartBeatMessage.TypeValue
            });
            Client.WebSocket.State.Returns(WebSocketState.Open);
            await Client.Connect();
            Enumerable.Repeat(0, 10).Select(async _ =>
            {
                await Client.WebSocket.Received(1).PingServer(Policy.PingMessage, Arg.Any<CancellationToken>());
                AdvanceTime(Policy.ExpectedPongInterval.Ticks - 5);
                await Task.Delay(150).ConfigureAwait(false);
                return 0;
            });
            await Client.WebSocket.DidNotReceive().RecycleConnectionAsync(Arg.Any<CancellationToken>());

            Client.WebSocket.State.Returns(WebSocketState.Closed);
        }

        [Fact]
        public async Task ApplyStrategy_should_reconnect_after_wait_pong_timeout_has_expired()
        {
            DateTimeProvider.UtcNow.Returns(DateTime.UtcNow);
            SimulateWebSocketResponse(new HeartBeatMessage
            {
                Timestamp = DateTime.UtcNow,
                Type = HeartBeatMessage.TypeValue
            });
            Client.WebSocket.State.Returns(WebSocketState.Open);
            await Client.Connect();
            await Client.WebSocket.Received(1).PingServer(Policy.PingMessage, Arg.Any<CancellationToken>());

            AdvanceTime(Policy.ExpectedPongInterval.Ticks);
            await Task.Delay(150).ConfigureAwait(false);
            await Client.WebSocket.Received(1).RecycleConnectionAsync(Arg.Any<CancellationToken>());

            Client.WebSocket.State.Returns(WebSocketState.Closed);
        }

    }
}
