using System;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Reactive.Testing;
using Newtonsoft.Json.Linq;
using NSubstitute;
using Trakx.WebSockets.KeepAlivePolicies;
using Trakx.WebSockets.Tests.Unit.DummyClient.Messages;
using Xunit;
using Xunit.Abstractions;

namespace Trakx.WebSockets.Tests.Unit
{
    public class PingPolicyTests : KeepAlivePolicyTestBase
    {

        private readonly TestScheduler _testScheduler;
        private readonly TimeSpan _keepAliveInterval;
        private readonly string _pingMessage;

        public PingPolicyTests(ITestOutputHelper output)
            : base(output)
        {
            _pingMessage = "ping server";
            _keepAliveInterval = TimeSpan.FromMinutes(3);
            _testScheduler = new TestScheduler();
            PingPolicy policy = new PingPolicy(_keepAliveInterval, _pingMessage, _testScheduler);
            ConfigureKeepAlivePolicy(policy);
        }

        [Fact]
        public async Task ApplyStrategy_should_ping_the_server_when_keep_alive_interval_elapses()
        {
            await Client.Connect();
            _testScheduler.AdvanceTo(_keepAliveInterval.Ticks + 1);
            await Client.WebSocket.Received(1).SendAsync(Arg.Any<ReadOnlyMemory<byte>>(), WebSocketMessageType.Text, 
                true, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task ApplyStrategy_should_not_ping_the_server_if_keep_alive_interval_was_not_elapsed()
        {
            await Client.Connect();
            _testScheduler.AdvanceTo(_keepAliveInterval.Ticks - 50);
            await Client.WebSocket.DidNotReceive().SendAsync(Arg.Any<ReadOnlyMemory<byte>>(), WebSocketMessageType.Text,
                true, Arg.Any<CancellationToken>());
        }

    }
}
