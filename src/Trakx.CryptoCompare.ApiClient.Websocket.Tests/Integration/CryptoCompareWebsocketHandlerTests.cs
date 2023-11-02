using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Trakx.CryptoCompare.ApiClient.Tests.Integration.Rest.Clients;
using Trakx.CryptoCompare.ApiClient.Websocket.Extensions;
using Trakx.CryptoCompare.ApiClient.Websocket.Model;
using Trakx.Websocket.Model;
using Xunit;

namespace Trakx.CryptoCompare.ApiClient.Websocket.Tests.Integration
{
    public class CryptoCompareWebsocketHandlerTests : IDisposable
    {
        private readonly IServiceScope _serviceScope;
        private readonly ICryptoCompareWebsocketHandler _cryptoCompareWebsocketHandler;

        public CryptoCompareWebsocketHandlerTests()
        {
            _serviceScope = CreateServiceProvider().CreateScope();
            _cryptoCompareWebsocketHandler = _serviceScope.ServiceProvider.GetRequiredService<ICryptoCompareWebsocketHandler>();
        }

        public static IServiceProvider CreateServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            var websocketConfiguration = new WebsocketConfiguration
            {
                BufferSize = 4096,
                MaxSubscriptionsPerScope = 100
            };

            var configuration = CryptoCompareApiFixture.LoadConfiguration();

            serviceCollection.AddCryptoCompareWebsockets(configuration, websocketConfiguration);
            return serviceCollection.BuildServiceProvider();
        }

        private async Task<T> GetResult<T>(string subStr, int bufferSeconds = 10)
            where T : InboundMessageBase
        {
            var topicSub = CryptoCompareSubscriptionFactory.GetTopicSubscription
                (SubscribeActions.SubAdd, subStr);

            await _cryptoCompareWebsocketHandler.AddAsync(topicSub);
            var res = await _cryptoCompareWebsocketHandler.GetTopicMessageStream<T>(topicSub.Topic)
                .Buffer(TimeSpan.FromSeconds(bufferSeconds), 1)
                .Select(t => t.FirstOrDefault())
                .FirstOrDefaultAsync()
                .ToTask();
            return res!;
        }


        [Fact]
        public async Task Should_be_able_to_get_full_top_tier_volume_subscriptions()
        {
            var subscriptionString = CryptoCompareSubscriptionFactory.GetFullTopTierVolumeSubscriptionStr("btc");
            var result = await GetResult<TopTierFullVolume>(subscriptionString);
            result!.Symbol.Should().Be("BTC");
            result.Volume.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Should_be_able_to_get_oc_book()
        {
            var subscriptionString = CryptoCompareSubscriptionFactory.GetTopOfOrderBookSubscriptionStr("Binance", "btc", "usdt");

            var result = await GetResult<TopOfOrderBook>(subscriptionString, bufferSeconds: 30);

            result.Should().NotBeNull();
            result!.Type.Should().Be("30");
        }

        [Fact]
        public async Task Should_be_able_to_get_ohlcc_candles()
        {
            var subscriptionString = CryptoCompareSubscriptionFactory.GetOHLCCandlesSubscriptionStr("Binance", "btc", "usdt", "m");
            var result = await GetResult<Ohlc>(subscriptionString);
            result!.Open.Should().BeGreaterThan(0);
            result!.LastTimeStamp.Should().BeGreaterThan(0);
            result!.Market.Should().NotBeNull();
            result.Close.Should().BeGreaterThan(0);
        }

        #region IDisposable

        private bool _wasDisposed;

        protected virtual bool Dispose(bool disposing)
        {
            if (_wasDisposed) return false;
            if (disposing)
            {
                _serviceScope.Dispose();
            }

            _wasDisposed = true;
            return true;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
