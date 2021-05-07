using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Reactive.Testing;
using NSubstitute;
using Trakx.Utils.DateTimeHelpers;
using Trakx.WebSockets.KeepAlivePolicies;
using Xunit;
using Xunit.Abstractions;

namespace Trakx.WebSockets.Tests.Unit
{
    public class PingPolicyTests : KeepAlivePolicyTestBase<PingPolicy>
    {

        private readonly TimeSpan _pingInterval;

        public PingPolicyTests(ITestOutputHelper output)
            : base(output)
        {
            var pingMessage = "ping server";
            _pingInterval = TimeSpan.FromMinutes(3);
            Policy = new PingPolicy(_pingInterval, pingMessage, 
                DateTimeProvider, TestScheduler);
            ConfigureKeepAlivePolicy(Policy);
        }

        [Fact]
        public async Task ApplyStrategy_should_ping_the_server_when_keep_alive_interval_elapses()
        {
            await Client.Connect();
            var times = 10;
            await Client.WebSocket.Received(1).PingServer(Policy.PingMessage, Arg.Any<CancellationToken>());
            TestScheduler.AdvanceTo(_pingInterval.Ticks * times);
            await Client.WebSocket.Received(1 + times).PingServer(Policy.PingMessage, Arg.Any<CancellationToken>());
        }

    }
}
