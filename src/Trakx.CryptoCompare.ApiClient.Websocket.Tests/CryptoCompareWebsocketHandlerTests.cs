using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text.Json;
using System.Threading.Tasks;
using Trakx.CryptoCompare.ApiClient.Websocket.Model;
using Trakx.Websocket.Interfaces;
using Trakx.Websocket.Model;
using Trakx.Websockets.Testing;
using Websocket.Client;
using Xunit;

namespace Trakx.CryptoCompare.ApiClient.Websocket.Tests
{
    public class CryptoCompareWebsocketHandlerTests
    {
        private TestWebsocketClient _testClient;
        private CryptoCompareWebsocketHandler _websocketHandler;

        public CryptoCompareWebsocketHandlerTests()
        {
            _testClient = Substitute.ForPartsOf<TestWebsocketClient>();
            var fakeOptions = Substitute.For<IOptions<WebsocketConfiguration>>();
            fakeOptions.Value.Returns(new WebsocketConfiguration
            {
                BufferSize = 4092,
                MaxSubscriptionsPerScope = 100
            });

            var fakeWebsocketFactory = Substitute.For<IClientWebsocketFactory>();
            fakeWebsocketFactory.CreateNewWebSocket(Arg.Any<Uri>(), Arg.Any<Action<WebsocketClient>>()).Returns(_testClient);
            var fakeFinexConfig = new CryptoCompareWebsocketConfiguration
            {
                Url = "https://www.google.com"
            };

            _websocketHandler = new CryptoCompareWebsocketHandler(fakeOptions, fakeFinexConfig, fakeWebsocketFactory);
        }

        [Fact]
        public async Task AddAsync_should_not_remove_subscription_on_sub_remove()
        {
            var topicSub = CryptoCompareSubscriptionFactory.GetTopicSubscription(SubscribeActions.SubAdd,
                CryptoCompareSubscriptionFactory.GetFullTopTierVolumeSubscriptionStr("test"));
            var removeSub = CryptoCompareSubscriptionFactory.GetTopicSubscription(SubscribeActions.SubRemove,
                CryptoCompareSubscriptionFactory.GetFullTopTierVolumeSubscriptionStr("test"));
            await _websocketHandler.AddAsync(topicSub);
            _websocketHandler.CurrentSubscriptions.Count.Should().Be(1);
            await _websocketHandler.AddAsync(removeSub);
            _websocketHandler.CurrentSubscriptions.Count.Should().Be(0);
        }

        [Fact]
        public async Task IncomingMessage_should_map_to_correct_observer()
        {
            var topicSub = CryptoCompareSubscriptionFactory.GetTopicSubscription(SubscribeActions.SubAdd,
              CryptoCompareSubscriptionFactory.GetFullTopTierVolumeSubscriptionStr("test"));
            await _websocketHandler.AddAsync(topicSub);

            var topicMessageTask = _websocketHandler.GetTopicMessageStream<FullVolume>(topicSub.Topic).Buffer(TimeSpan.FromSeconds(5), 1)
                .Select(t => t.FirstOrDefault())
                .FirstOrDefaultAsync()
                .ToTask();
            var fakeMsg = JsonSerializer.Serialize(new FullVolume { Type = "11", Volume = 1, Symbol = "BTC" });
            _testClient.StreamFakeMessage(ResponseMessage.TextMessage(fakeMsg));

            var topicMessage = await topicMessageTask;
            topicMessage.Should().NotBeNull();

        }
    }
}
