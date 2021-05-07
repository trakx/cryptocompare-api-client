using System;
using Trakx.CryptoCompare.ApiClient.WebSocket.DTOs.Inbound;
using Trakx.WebSockets;

namespace Trakx.CryptoCompare.ApiClient.WebSocket
{
    public  interface ICryptoCompareWebSocketStreamer : IWebSocketStreamer<InboundMessageBase>
    {

        IObservable<InboundMessageBase> AllInboundMessagesStream { get; }

        IObservable<Trade> TradeStream { get; }

        IObservable<Ticker> TickerStream { get; }

        IObservable<AggregateIndex> AggregateIndexStream { get; }

        IObservable<Ohlc> OhlcStream { get; }

        IObservable<SubscribeComplete> SubscribeCompleteStream { get; }

        IObservable<UnsubscribeComplete> UnsubscribeCompleteStream { get; }

        IObservable<LoadComplete> LoadCompleteStream { get; }

        IObservable<UnsubscribeAllComplete> UnsubscribeAllCompleteStream { get; }

        IObservable<HeartBeat> HeartBeatStream { get; }

        IObservable<Error> ErrorStream { get; }


    }
}
