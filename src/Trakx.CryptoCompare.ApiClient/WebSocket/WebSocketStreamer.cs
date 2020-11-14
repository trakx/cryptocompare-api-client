using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text.Json;
using Serilog;
using Trakx.CryptoCompare.ApiClient.WebSocket.DTOs.Inbound;

namespace Trakx.CryptoCompare.ApiClient.WebSocket
{
    public interface IWebSocketStreamer
    {
        IObservable<InboundMessageBase> AllInboundMessagesStream { get; }
        IObservable<Trade> TradeStream { get; }
        IObservable<Ticker> TickerStream { get; }
        IObservable<AggregateIndex> AggregateIndiceStream { get; }
        IObservable<Ohlc> OhlcStream { get; }
        IObservable<SubscribeComplete> SubscribeCompleteStream { get; }
        IObservable<UnsubscribeComplete> UnsubscribeCompleteStream { get; }
        IObservable<LoadComplete> LoadCompleteStream { get; }
        IObservable<UnsubscribeAllComplete> UnsubscribeAllCompleteStream { get; }
        IObservable<HeartBeat> HeartBeatStream { get; }
        IObservable<Error> ErrorStream { get; }
        void PublishInboundMessageOnStream(string rawMessage);
    }

    public class WebSocketStreamer : IWebSocketStreamer
    {
        private readonly ILogger _logger;
        private readonly ISubject<InboundMessageBase> _incomingMessageSubject;
        
        public WebSocketStreamer(ILogger logger)
        {
            _logger = logger;
            _incomingMessageSubject = new ReplaySubject<InboundMessageBase>(1);
        }

        public IObservable<InboundMessageBase> AllInboundMessagesStream => _incomingMessageSubject.AsObservable();
        public IObservable<Trade> TradeStream => _incomingMessageSubject.OfType<Trade>().AsObservable();
        public IObservable<Ticker> TickerStream => _incomingMessageSubject.OfType<Ticker>().AsObservable();
        public IObservable<AggregateIndex> AggregateIndiceStream => _incomingMessageSubject.OfType<AggregateIndex>().AsObservable();
        public IObservable<Ohlc> OhlcStream => _incomingMessageSubject.OfType<Ohlc>().AsObservable();
        public IObservable<SubscribeComplete> SubscribeCompleteStream => _incomingMessageSubject.OfType<SubscribeComplete>().AsObservable();
        public IObservable<UnsubscribeComplete> UnsubscribeCompleteStream => _incomingMessageSubject.OfType<UnsubscribeComplete>().AsObservable();
        public IObservable<LoadComplete> LoadCompleteStream => _incomingMessageSubject.OfType<LoadComplete>().AsObservable();
        public IObservable<UnsubscribeAllComplete> UnsubscribeAllCompleteStream => _incomingMessageSubject.OfType<UnsubscribeAllComplete>().AsObservable();
        public IObservable<HeartBeat> HeartBeatStream => _incomingMessageSubject.OfType<HeartBeat>().AsObservable();
        public IObservable<Error> ErrorStream => _incomingMessageSubject.OfType<Error>().AsObservable();

        public void PublishInboundMessageOnStream(string rawMessage)
        {
            try
            {
                _logger.Verbose("Received WebSocketInboundMessage {0}{1}", Environment.NewLine, rawMessage);
                var message = JsonSerializer.Deserialize<InboundMessageBase>(rawMessage);
                if (message == default)
                {
                    _logger.Warning("Failed to parse message {rawMessage} as a known InboundMessageBase", rawMessage);
                    return;
                }
                switch (message.Type)
                {
                    case Trade.TypeValue:
                        _incomingMessageSubject.OnNext(JsonSerializer.Deserialize<Trade>(rawMessage)!);
                        break;
                    case Ticker.TypeValue:
                        _incomingMessageSubject.OnNext(JsonSerializer.Deserialize<Ticker>(rawMessage)!);
                        break;
                    case AggregateIndex.TypeValue:
                        _incomingMessageSubject.OnNext(JsonSerializer.Deserialize<AggregateIndex>(rawMessage)!);
                        break;
                    case Ohlc.TypeValue:
                        _incomingMessageSubject.OnNext(JsonSerializer.Deserialize<Ohlc>(rawMessage)!);
                        break;
                    case SubscribeComplete.TypeValue:
                        _incomingMessageSubject.OnNext(JsonSerializer.Deserialize<SubscribeComplete>(rawMessage)!);
                        break;
                    case UnsubscribeComplete.TypeValue:
                        _incomingMessageSubject.OnNext(JsonSerializer.Deserialize<SubscribeComplete>(rawMessage)!);
                        break;
                    case LoadComplete.TypeValue:
                        _incomingMessageSubject.OnNext(JsonSerializer.Deserialize<LoadComplete>(rawMessage)!);
                        break;
                    case UnsubscribeAllComplete.TypeValue:
                        _incomingMessageSubject.OnNext(JsonSerializer.Deserialize<SubscribeComplete>(rawMessage)!);
                        break;
                    case HeartBeat.TypeValue:
                        _incomingMessageSubject.OnNext(JsonSerializer.Deserialize<HeartBeat>(rawMessage)!);
                        break;
                    case Error.TypeValue:
                        _incomingMessageSubject.OnNext(JsonSerializer.Deserialize<Error>(rawMessage)!);
                        break;
                    default:
                        return;
                }
            }
            catch (Exception exception)
            {
                _logger.Error(exception, "Failed to publish {0}", rawMessage);
            }
        }
    }
}