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
using Xunit;
using Xunit.Abstractions;

namespace Trakx.WebSockets.Tests.Unit
{
    public class KeepAlivePolicyTestBase
    {

        protected WebSocketClient<BaseInboundMessage, IDummyWebSocketStreamer> Client;
        protected readonly IWebSocketAdapter WebSocketAdapter;
        protected readonly IDummyWebSocketStreamer WebSocketStreamer;
        protected readonly IDateTimeProvider DateTimeProvider;

        public KeepAlivePolicyTestBase(ITestOutputHelper output)
        {
            DateTimeProvider = Substitute.For<IDateTimeProvider>();
            WebSocketAdapter = Substitute.For<IWebSocketAdapter>();
            WebSocketStreamer = Substitute.For<IDummyWebSocketStreamer>();
        }

        public void ConfigureKeepAlivePolicy(IKeepAlivePolicy policy)
        {
            Client = new DummyWebSocketClient(WebSocketAdapter, "ws://abc",
                policy, WebSocketStreamer);
        }

        protected void SimulateWebSocketResponse(BaseInboundMessage message, bool isCloseMessage = false)
        {
            WebSocketAdapter.State.Returns(WebSocketState.Open, WebSocketState.Open, WebSocketState.Open, WebSocketState.Open, WebSocketState.Open, WebSocketState.Open, WebSocketState.Open, WebSocketState.Closed);
            var messageBytes = Encoding.UTF8.GetBytes(JObject.FromObject(message).ToString()).AsMemory();
            WebSocketAdapter.ReceiveAsync(Arg.Any<ArraySegment<byte>>(), Arg.Any<CancellationToken>())
                .Returns(async ci =>
                {
                    ((CancellationToken)ci[1]).ThrowIfCancellationRequested();
                    await Task.Delay(100).ConfigureAwait(false);
                    var webSocketMessageType = isCloseMessage ? WebSocketMessageType.Close : WebSocketMessageType.Text;
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



    }
}
