using System;
using Trakx.WebSockets.Tests.Unit.DummyClient.Messages;

namespace Trakx.WebSockets.Tests.Unit.DummyClient
{
    public interface IDummyWebSocketStreamer : IWebSocketStreamer<BaseInboundMessage>
    {
        
        IObservable<BaseInboundMessage> AllInboundMessagesStream { get; }

        IObservable<HeartBeatMessage> HeartBeatStream { get; }
        
        IObservable<PriceChangedMessage> PriceChangedStream { get; }

    }

}
