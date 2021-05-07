using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Trakx.CryptoCompare.ApiClient.WebSocket.DTOs.Inbound;
using Trakx.WebSockets;

namespace Trakx.CryptoCompare.ApiClient.WebSocket
{
    public class CryptoCompareWebSocketStreamer : WebSocketStreamer<InboundMessageBase>, ICryptoCompareWebSocketStreamer
    {

        public IObservable<InboundMessageBase> AllInboundMessagesStream => InboundMessages.AsObservable();
        public IObservable<Trade> TradeStream => InboundMessages.OfType<Trade>().AsObservable();
        public IObservable<Ticker> TickerStream => InboundMessages.OfType<Ticker>().AsObservable();
        public IObservable<AggregateIndex> AggregateIndexStream => InboundMessages.OfType<AggregateIndex>().AsObservable();
        public IObservable<Ohlc> OhlcStream => InboundMessages.OfType<Ohlc>().AsObservable();
        public IObservable<SubscribeComplete> SubscribeCompleteStream => InboundMessages.OfType<SubscribeComplete>().AsObservable();
        public IObservable<UnsubscribeComplete> UnsubscribeCompleteStream => InboundMessages.OfType<UnsubscribeComplete>().AsObservable();
        public IObservable<LoadComplete> LoadCompleteStream => InboundMessages.OfType<LoadComplete>().AsObservable();
        public IObservable<UnsubscribeAllComplete> UnsubscribeAllCompleteStream => InboundMessages.OfType<UnsubscribeAllComplete>().AsObservable();
        public IObservable<HeartBeat> HeartBeatStream => InboundMessages.OfType<HeartBeat>().AsObservable();
        public IObservable<Error> ErrorStream => InboundMessages.OfType<Error>().AsObservable();

        public override Type? GetMessageType(string typeName)
        {
            if (typeName == Trade.TypeValue) return typeof(Trade);
            if (typeName == Ticker.TypeValue) return typeof(Ticker);
            if (typeName == AggregateIndex.TypeValue) return typeof(AggregateIndex);
            if (typeName == Ohlc.TypeValue) return typeof(Ohlc);
            if (typeName == SubscribeComplete.TypeValue) return typeof(SubscribeComplete);
            if (typeName == UnsubscribeComplete.TypeValue) return typeof(UnsubscribeComplete);
            if (typeName == LoadComplete.TypeValue) return typeof(LoadComplete);
            if (typeName == UnsubscribeAllComplete.TypeValue) return typeof(UnsubscribeAllComplete);
            if (typeName == HeartBeat.TypeValue) return typeof(HeartBeat);
            if (typeName == Error.TypeValue) return typeof(Error);
            return null;
        }

        public override Dictionary<string, IObservable<InboundMessageBase>> GetStreams()
        {
            return new()
            {
                { Trade.TypeValue, TradeStream },
                { Ticker.TypeValue, TickerStream },
                { AggregateIndex.TypeValue, AggregateIndexStream },
                { Ohlc.TypeValue, OhlcStream },
                { SubscribeComplete.TypeValue, SubscribeCompleteStream },
                { UnsubscribeComplete.TypeValue, UnsubscribeCompleteStream },
                { LoadComplete.TypeValue, LoadCompleteStream },
                { UnsubscribeAllComplete.TypeValue, UnsubscribeAllCompleteStream },
                { HeartBeat.TypeValue, HeartBeatStream },
                { Error.TypeValue, ErrorStream },
            };
        }

        public override IObservable<TMessageType> GetStream<TMessageType>(string topicName)
        {
            var topics = GetStreams();
            return topics.ContainsKey(topicName) ? topics[topicName].Cast<TMessageType>() : null;
        }


    }
}
