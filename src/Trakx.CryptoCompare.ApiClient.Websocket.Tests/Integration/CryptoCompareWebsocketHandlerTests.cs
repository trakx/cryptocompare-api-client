using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Trakx.CryptoCompare.ApiClient.Tests;
using Trakx.CryptoCompare.ApiClient.Websocket.Extensions;
using Trakx.CryptoCompare.ApiClient.Websocket.Model;
using Trakx.Websocket;
using Trakx.Websocket.Interfaces;
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

        public IServiceProvider CreateServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IClientWebsocketFactory, ClientWebsocketFactory>();
            serviceCollection.AddSingleton(Options.Create(new WebsocketConfiguration
            {
                BufferSize = 4096,
                MaxSubscriptionsPerScope = 100
            }));
            serviceCollection.AddCryptoCompareWebsocketHandler(new CryptoCompareWebsocketConfiguration
            {
                Url = "wss://streamer.cryptocompare.com/v2?api_key=",
                ApiKey = new Secrets().ApiKey
            });
            return serviceCollection.BuildServiceProvider();
        }

        public void Dispose()
        {
            _serviceScope.Dispose();
        }


        private async Task<T> GetResult<T>(string subStr) where T : InboundMessageBase
        {
            var topicSub = CryptoCompareSubscriptionFactory.GetTopicSubscription
                (SubscribeActions.SubAdd, subStr);
           
            await _cryptoCompareWebsocketHandler.AddAsync(topicSub);
            var resultTask = _cryptoCompareWebsocketHandler.GetTopicMessageStream<T>(topicSub.Topic)
                .Buffer(TimeSpan.FromSeconds(10), 1)
                .Select(t => t.FirstOrDefault())
                .FirstOrDefaultAsync()
                .ToTask();
            var result = await resultTask;
            return result!;
        }


        [Fact]
        public async Task Should_be_able_to_get_full_top_tier_volume_subscriptions()
        {
            var result = await GetResult<TopTierFullVolume>(CryptoCompareSubscriptionFactory.GetFullTopTierVolumeSubscriptionStr("btc"));
            result!.Symbol.Should().Be("BTC");
            decimal.Parse(result.Volume).Should().BeGreaterThan(0); 
        }

        [Fact]
        public async Task Should_be_able_to_get_oc_book()
        {
            var result = await GetResult<TopOfOrderBook>(CryptoCompareSubscriptionFactory.GetTopOfOrderBookSubscriptionStr("Binance", "btc", "usdt"));
            result!.Type.Should().Be("30");
        }

        [Fact]
        public async Task Should_be_able_to_get_ohlcc_candles()
        {
            var result = await GetResult<Ohlc>(CryptoCompareSubscriptionFactory.GetOHLCCandlesSubscriptionStr("Binance", "btc", "usdt", "m"));
            result!.Open.Should().BeGreaterThan(0);
            result!.LastTimeStamp.Should().BeGreaterThan(0);
            result!.Market.Should().NotBeNull();
            result.Close.Should().BeGreaterThan(0);
        }
    }
}
