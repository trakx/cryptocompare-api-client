using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Trakx.CryptoCompare.ApiClient.Websocket.Model;
using Trakx.Websocket.Interfaces;
using Trakx.Websocket.Model;
using Trakx.Websockets.Testing;
using Websocket.Client;
using Xunit;

namespace Trakx.CryptoCompare.ApiClient.Websocket.Tests.Unit;

public class CryptoCompareWebsocketHandlerTests
{
    private const string TestSymbol = "btc";

    private readonly TestWebsocketClient _testClient;
    private readonly CryptoCompareWebsocketHandler _websocketHandler;
    private readonly TopicSubscription _topicSub;
    private readonly TopicSubscription _removeSub;

    private int SubscriptionCount => _websocketHandler.CurrentSubscriptions.Count;

    public CryptoCompareWebsocketHandlerTests()
    {
        _testClient = Substitute.ForPartsOf<TestWebsocketClient>();

        var fakeConfiguration = new WebsocketConfiguration
        {
            BufferSize = 4092,
            MaxSubscriptionsPerScope = 100
        };

        var fakeWebsocketFactory = Substitute.For<IClientWebsocketFactory>();
        fakeWebsocketFactory.CreateNewWebSocket(Arg.Any<Uri>(), Arg.Any<Action<WebsocketClient>>())
            .Returns(_testClient);

        var fakeFinexConfig = new CryptoCompareApiConfiguration()
        {
            WebSocketBaseUrl = "https://www.google.com"
        };

        _websocketHandler = new CryptoCompareWebsocketHandler(fakeConfiguration, fakeFinexConfig, fakeWebsocketFactory);

        _topicSub = SetupTopicSubscription(SubscribeActions.SubAdd);
        _removeSub = SetupTopicSubscription(SubscribeActions.SubRemove);
    }

    [Fact]
    public async Task AddAsync_sends_message_to_client()
    {
        await _websocketHandler.AddAsync(_topicSub);
        _testClient.Received(1).Send(_topicSub.Topic);
    }

    [Fact]
    public async Task RemoveAsync_sends_message_to_client()
    {
        // this call is needed to "open" the channel
        await _websocketHandler.AddAsync(_topicSub);

        await _websocketHandler.RemoveAsync(_removeSub);

        _testClient.Received(1).Send(_removeSub.Topic);
    }

    [Fact]
    public async Task AddAsync_removes_subscription_on_sub_remove()
    {
        await _websocketHandler.AddAsync(_topicSub);
        SubscriptionCount.Should().Be(1);

        await _websocketHandler.AddAsync(_removeSub);
        SubscriptionCount.Should().Be(0);
    }

    [Fact]
    public async Task RemoveAsync_only_removes_subscription_with_valid_topic()
    {
        await _websocketHandler.AddAsync(_topicSub);
        SubscriptionCount.Should().Be(1);

        // no changes because it doesn't have the SubRemove action
        await _websocketHandler.RemoveAsync(_topicSub);
        SubscriptionCount.Should().Be(1);

        await _websocketHandler.RemoveAsync(_removeSub);
        SubscriptionCount.Should().Be(0);

        // only 2 message should have been sent, the first Add and the valid Remove
        _testClient.Received(1).Send(_topicSub.Topic);
        _testClient.Received(1).Send(_removeSub.Topic);
    }

    [Fact]
    public async Task IncomingMessage_should_map_to_correct_observer()
    {
        await _websocketHandler.AddAsync(_topicSub);

        var topicMessageTask = _websocketHandler.GetTopicMessageStream<FullVolume>(_topicSub.Topic)
            .Buffer(TimeSpan.FromSeconds(5), 1)
            .Select(t => t.FirstOrDefault())
            .FirstOrDefaultAsync()
            .ToTask();
        var fullVolumeMsg = new FullVolume { Type = "11", Volume = 1, Symbol = TestSymbol };
        var fakeMsg = JsonSerializer.Serialize(fullVolumeMsg);
        _testClient.StreamFakeMessage(ResponseMessage.TextMessage(fakeMsg));

        var topicMessage = await topicMessageTask;
        topicMessage!.Type.Should().Be(fullVolumeMsg.Type);
        topicMessage!.Symbol.Should().Be(fullVolumeMsg.Symbol);
        topicMessage!.Volume.Should().Be(fullVolumeMsg.Volume);
    }

    private static TopicSubscription SetupTopicSubscription(SubscribeActions action)
    {
        var subscriptionString = CryptoCompareSubscriptionFactory.GetFullTopTierVolumeSubscriptionStr(TestSymbol);
        return CryptoCompareSubscriptionFactory.GetTopicSubscription(action, subscriptionString);
    }
}
