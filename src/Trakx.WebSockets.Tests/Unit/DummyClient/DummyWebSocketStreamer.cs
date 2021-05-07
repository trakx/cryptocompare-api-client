using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Trakx.WebSockets.Tests.Unit.DummyClient.Messages;

namespace Trakx.WebSockets.Tests.Unit.DummyClient
{
    public class DummyWebSocketStreamer : WebSocketStreamer<BaseInboundMessage>, IDummyWebSocketStreamer
    {
        public override Type? GetMessageType(string typeName)
        {
            if (HeartBeatMessage.TypeValue == typeName) return typeof(HeartBeatMessage);
            if (PriceChangedMessage.TypeValue == typeName) return typeof(PriceChangedMessage);
            return default;
        }

        public IObservable<BaseInboundMessage> AllInboundMessagesStream => InboundMessages.AsObservable();

        public IObservable<HeartBeatMessage> HeartBeatStream => InboundMessages.OfType<HeartBeatMessage>().AsObservable();

        public IObservable<PriceChangedMessage> PriceChangedStream => InboundMessages.OfType<PriceChangedMessage>().AsObservable();

        public override Dictionary<string, IObservable<BaseInboundMessage>> GetStreams()
        {
            return new()
            {
                {HeartBeatMessage.TypeValue, HeartBeatStream},
                {PriceChangedMessage.TypeValue, PriceChangedStream}
            };
        }

        public override IObservable<TMessageType> GetStream<TMessageType>(string topicName)
        {
            var streams = GetStreams();
            return streams.ContainsKey(topicName) ? streams[topicName].Cast<TMessageType>() : null;
        }

    }
}
