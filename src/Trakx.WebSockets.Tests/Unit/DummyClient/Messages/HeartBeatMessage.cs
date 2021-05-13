using System;

namespace Trakx.WebSockets.Tests.Unit.DummyClient.Messages
{
    public class HeartBeatMessage : BaseInboundMessage
    {
        public HeartBeatMessage() : base(TypeValue) { }
        
        public static readonly string TypeValue = "HeartBeat";

        public DateTime Timestamp { get; set; }

    }
}
