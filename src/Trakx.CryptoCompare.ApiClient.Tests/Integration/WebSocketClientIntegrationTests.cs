using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Serilog;
using Trakx.CryptoCompare.ApiClient.WebSocket;
using Trakx.CryptoCompare.ApiClient.WebSocket.DTOs.Inbound;
using Trakx.CryptoCompare.ApiClient.WebSocket.DTOs.Outbound;
using Xunit;
using Xunit.Abstractions;

namespace Trakx.CryptoCompare.ApiClient.Tests.Integration
{
    //AsyncDispose not yet supported by XUnit so let's implement both...
    public sealed class WebSocketClientIntegrationTests : IAsyncDisposable, IDisposable
    {
        private readonly ITestOutputHelper _output;
        private readonly CryptoCompareWebSocketClient _client;

        public WebSocketClientIntegrationTests(ITestOutputHelper output)
        {
            _output = output;
            var logger = new LoggerConfiguration().WriteTo.TestOutput(output).CreateLogger()
                .ForContext(MethodBase.GetCurrentMethod()!.DeclaringType);
            var streamer = new WebSocketStreamer(logger);
            var apiDetailsProvider = new ApiDetailsProvider(Secrets.ApiKey);
            var clientWebSocket = new WrappedClientWebsocket();
            _client = new CryptoCompareWebSocketClient(clientWebSocket, apiDetailsProvider, streamer, logger);
        }

        [Fact]
#pragma warning disable S2699 // Tests should include assertions
        public async Task WebSocketClient_should_receive_Trade_updates()
#pragma warning restore S2699 // Tests should include assertions
        {
            var btcUsdSubscription = new TradeSubscription("Coinbase", "btc", "usd");
            await RunTestForSubscriptionType<Trade>(btcUsdSubscription);
        }

        [Fact]
#pragma warning disable S2699 // Tests should include assertions
        public async Task WebSocketClient_should_receive_Ticker_updates()
#pragma warning restore S2699 // Tests should include assertions
        {
            var btcUsdSubscription = new TickerSubscription("Bitfinex", "eth", "usd");
            await RunTestForSubscriptionType<Ticker>(btcUsdSubscription);
        }

        [Fact]
#pragma warning disable S2699 // Tests should include assertions
        public async Task WebSocketClient_should_receive_AggregateIndice_updates()
#pragma warning restore S2699 // Tests should include assertions
        {
            var btcUsdSubscription = new AggregateIndexSubscription("btc", "usd");
            await RunTestForSubscriptionType<AggregateIndex>(btcUsdSubscription);
        }

        [Fact]
#pragma warning disable S2699 // Tests should include assertions
        public async Task WebSocketClient_should_receive_Ohlc_updates()
#pragma warning restore S2699 // Tests should include assertions
        {
            var btcUsdSubscription = new OhlcSubscription("CCCAGG", "eth", "usd", TimeSpan.FromHours(1));
            await RunTestForSubscriptionType<Ohlc>(btcUsdSubscription);
        }

        private async Task RunTestForSubscriptionType<T>(ICryptoCompareSubscription subscription)
        {
            await _client.Connect();
            _client.State.Should().Be(WebSocketState.Open);

            var messagesReceived = new List<InboundMessageBase>();

            using var inboundMessageStream = _client.WebSocketStreamer.AllInboundMessagesStream
                .SubscribeOn(Scheduler.Default)
                .Take(50)
                .Subscribe(m =>
                {
                    _output.WriteLine(JsonSerializer.Serialize(m));
                    messagesReceived.Add(m);
                });


            await _client.AddSubscriptions(subscription).ConfigureAwait(false);
            await Task.Delay(TimeSpan.FromMilliseconds(500));

            messagesReceived.Count.Should().BeGreaterOrEqualTo(3);

            messagesReceived.OfType<T>().Count().Should().BeGreaterOrEqualTo(1);
            messagesReceived.OfType<SubscribeComplete>().Count().Should().BeGreaterOrEqualTo(1);
            messagesReceived.OfType<LoadComplete>().Count().Should().BeGreaterOrEqualTo(1);
            
            await _client.RemoveSubscriptions(subscription).ConfigureAwait(false);
            await Task.Delay(TimeSpan.FromMilliseconds(500));

            messagesReceived.Count.Should().BeGreaterOrEqualTo(5);
            messagesReceived.OfType<SubscribeComplete>().Count().Should().BeGreaterOrEqualTo(1);
            messagesReceived.OfType<LoadComplete>().Count().Should().BeGreaterOrEqualTo(1);

            await _client.DisposeAsync();
            _client.State.Should().Be(WebSocketState.Closed);
        }

        #region IDisposable

        /// <inheritdoc />
        public async ValueTask DisposeAsync()
        {
            await _client.DisposeAsync();
        }

        #endregion

        #region IDisposable

        /// <inheritdoc />
        public void Dispose()
        {
            try
            {
                DisposeAsync().GetAwaiter().GetResult();
            }
            catch
            {
                //the try part is only needed if the test fails
            }
        }

        #endregion
    }
}
