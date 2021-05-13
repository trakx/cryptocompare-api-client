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
    public class HeartBeatPolicyTests : KeepAlivePolicyTestBase<HeartBeatPolicy>
    {

        private readonly TimeSpan _maxDuration;

        public HeartBeatPolicyTests(ITestOutputHelper output)
            : base(output)
        {
            _maxDuration = TimeSpan.FromMinutes(3);
            Policy = new HeartBeatPolicy(HeartBeatMessage.TypeValue,
                _maxDuration, DateTimeProvider, TestScheduler);
            ConfigureKeepAlivePolicy(Policy);
        }

        [Fact]
        public async Task ApplyStrategy_should_recycle_connection_if_heartbeat_stream_is_not_triggered_after_max_duration()
        {
            Client.WebSocket.State.Returns(WebSocketState.Open);
            SimulateJsonResponse(new HeartBeatMessage
            {
                Timestamp = DateTime.UtcNow,
            });
            await Client.Connect();
            AdvanceTime(_maxDuration.Add(TimeSpan.FromSeconds(1)).Ticks);
            await FlushData();
            await Task.Delay(150).ConfigureAwait(false);
            await Client.WebSocket.Received(1).RecycleConnectionAsync(Arg.Any<CancellationToken>());
            Client.WebSocket.State.Returns(WebSocketState.Closed);
        }

    }
}
