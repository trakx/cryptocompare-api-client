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
            var pongTimeout = TimeSpan.FromMinutes(30);
            Policy = new PingPongPolicy(pingMessage,
                pongTimeout, expectedPongMessage, DateTimeProvider,
                TestScheduler);
            ConfigureKeepAlivePolicy(Policy);
        }

        [Fact]
        public async Task ApplyStrategy_should_not_reconnect_if_ping_pong_logic_intervals_are_respected()
        {
            SimulateJsonResponse(new HeartBeatMessage
            {
                Timestamp = DateTime.UtcNow,
                Type = HeartBeatMessage.TypeValue
            });
            Client.WebSocket.State.Returns(WebSocketState.Open);

            await Client.Connect();
            await Client.WebSocket.Received().PingServer(Policy.PingMessage, Arg.Any<CancellationToken>());

            var repeat = 10;
            while (repeat > 0)
            {
                var lastPongTime = Policy.LastPongDateTime;

                Client.WebSocket.ClearReceivedCalls();
                SimulateRawResponse(Policy.ExpectedPongMessage);
                AdvanceTime(Policy.PongTimeout.Add(TimeSpan.FromSeconds(-2)).Ticks);

                while (lastPongTime == Policy.LastPongDateTime) { await Task.Delay(5); }

                await Client.WebSocket.DidNotReceive().RecycleConnectionAsync(Arg.Any<CancellationToken>());
                await Client.WebSocket.Received().PingServer(Policy.PingMessage, Arg.Any<CancellationToken>());
                repeat--;
            }

            Client.WebSocket.State.Returns(WebSocketState.Closed);
        }

        [Fact]
        public async Task ApplyStrategy_should_reconnect_after_wait_pong_timeout_has_expired()
        {
            SimulateJsonResponse(new HeartBeatMessage
            {
                Timestamp = DateTime.UtcNow,
                Type = HeartBeatMessage.TypeValue
            });
            Client.WebSocket.State.Returns(WebSocketState.Open);

            await Client.Connect();
            await Client.WebSocket.Received().PingServer(Policy.PingMessage, Arg.Any<CancellationToken>());

            AdvanceTime(Policy.PongTimeout.Add(TimeSpan.FromSeconds(30)).Ticks);

            await Task.Delay(300).ConfigureAwait(false);

            await Client.WebSocket.Received().RecycleConnectionAsync(Arg.Any<CancellationToken>());

            Client.WebSocket.State.Returns(WebSocketState.Closed);
        }

    }
}
