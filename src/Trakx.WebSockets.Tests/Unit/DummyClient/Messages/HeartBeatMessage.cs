using System;

namespace Trakx.WebSockets.Tests.Unit.DummyClient.Messages
{
    public class HeartBeatMessage : BaseInboundMessage
    {

        public static readonly string TypeValue = "HeartBeat";

        public string Type { get; set; }

        public DateTime Timestamp { get; set; }

    }
}
