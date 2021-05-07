using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Reactive.Testing;
using NSubstitute;
using Trakx.Utils.DateTimeHelpers;
using Trakx.WebSockets.KeepAlivePolicies;
using Trakx.WebSockets.Tests.Unit.DummyClient.Messages;
using Xunit;
using Xunit.Abstractions;

namespace Trakx.WebSockets.Tests.Unit
{
    public class HeartBeatPolicyTests : KeepAlivePolicyTestBase
    {

        private readonly TestScheduler _testScheduler;
        private readonly TimeSpan _maxDuration;

        public HeartBeatPolicyTests(ITestOutputHelper output) 
            : base(output)
        {
            _maxDuration = TimeSpan.FromMinutes(3);
            _testScheduler = new TestScheduler();
            ConfigureKeepAlivePolicy(new HeartBeatPolicy(HeartBeatMessage.TypeValue,
                _maxDuration, new DateTimeProvider(), _testScheduler));
        }

        [Fact]
        public async Task ApplyStrategy_should_recycle_connection_if_heartbeat_stream_is_not_triggered_after_max_duration()
        {
            DateTimeProvider.UtcNow.Returns(DateTime.UtcNow, DateTime.UtcNow.AddTicks(_maxDuration.Ticks + 1));
            SimulateWebSocketResponse(new PriceChangedMessage
            {
                Symbol = "abc",
                Price = (decimal)1.99,
                Timestamp = DateTime.UtcNow,
                Type = PriceChangedMessage.TypeValue
            });
            await Client.Connect();
            _testScheduler.AdvanceTo(_maxDuration.Ticks + 1);
            while (!Client.Streamer.ReceivedCalls().Any())
            {
                await Task.Delay(10).ConfigureAwait(false);
            }
            // todo: to implement logic to test policy
        }

    }
}
