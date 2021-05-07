using System;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Reactive.Testing;
using Newtonsoft.Json.Linq;
using NSubstitute;
using Trakx.Utils.DateTimeHelpers;
using Trakx.WebSockets.KeepAlivePolicies;
using Trakx.WebSockets.Tests.Unit.DummyClient;
using Trakx.WebSockets.Tests.Unit.DummyClient.Messages;
using Xunit.Abstractions;

namespace Trakx.WebSockets.Tests.Unit
{
    public class KeepAlivePolicyTestBase<TPolicy>
    where TPolicy : IKeepAlivePolicy
    {

        protected WebSocketClient<BaseInboundMessage, IDummyWebSocketStreamer> Client;
        protected readonly IWebSocketAdapter WebSocketAdapter;
        protected readonly IDummyWebSocketStreamer WebSocketStreamer;
        protected readonly IDateTimeProvider DateTimeProvider;
        protected readonly TestScheduler TestScheduler;
        protected TPolicy Policy { get; init; }

        public KeepAlivePolicyTestBase(ITestOutputHelper output)
        {
            DateTimeProvider = Substitute.For<IDateTimeProvider>();
            WebSocketAdapter = Substitute.For<IWebSocketAdapter>();
            WebSocketStreamer = Substitute.For<IDummyWebSocketStreamer>();
            TestScheduler = new TestScheduler();
            DateTimeProvider.UtcNow.Returns(DateTime.UtcNow);
        }

        public void ConfigureKeepAlivePolicy(IKeepAlivePolicy policy)
        {
            Client = new DummyWebSocketClient(WebSocketAdapter, "ws://abc",
                policy, WebSocketStreamer);
        }

        protected void SimulateJsonResponse(BaseInboundMessage message)
        {
            var messageBytes = Encoding.UTF8.GetBytes(JObject.FromObject(message).ToString()).AsMemory();
            WebSocketAdapter.ReceiveAsync(Arg.Any<ArraySegment<byte>>(), Arg.Any<CancellationToken>())
                .Returns(async ci =>
                {
                    ((CancellationToken)ci[1]).ThrowIfCancellationRequested();
                    await Task.Delay(100).ConfigureAwait(false);
                    var webSocketMessageType = WebSocketMessageType.Text;
                    return new WebSocketReceiveResult(messageBytes.Length, webSocketMessageType, true);
                })
                .AndDoes(ci =>
                {
                    var buffer = (ArraySegment<byte>)ci[0];
                    messageBytes.TryCopyTo(buffer);
                });
        }

        protected void SimulateRawResponse(string rawMessage)
        {
            var messageBytes = Encoding.UTF8.GetBytes(rawMessage).AsMemory();
            WebSocketAdapter.ReceiveAsync(Arg.Any<ArraySegment<byte>>(), Arg.Any<CancellationToken>())
                .Returns(async ci =>
                {
                    ((CancellationToken)ci[1]).ThrowIfCancellationRequested();
                    await Task.Delay(50).ConfigureAwait(false);
                    var webSocketMessageType = WebSocketMessageType.Text;
                    return new WebSocketReceiveResult(messageBytes.Length, webSocketMessageType, true);
                })
                .AndDoes(ci =>
                {
                    var buffer = (ArraySegment<byte>)ci[0];
                    messageBytes.TryCopyTo(buffer);
                });
        }

        protected async Task FlushData()
        {
            while (!Client.Streamer.ReceivedCalls().Any())
            {
                await Task.Delay(10).ConfigureAwait(false);
            }
        }

        protected void AdvanceTime(long ticks)
        {
            DateTimeProvider.UtcNow.Returns(DateTimeProvider.UtcNow.AddTicks(ticks));
            TestScheduler.AdvanceTo(ticks);
        }

    }
}