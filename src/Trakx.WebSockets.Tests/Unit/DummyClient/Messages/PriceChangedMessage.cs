using System;

namespace Trakx.WebSockets.Tests.Unit.DummyClient.Messages
{
    public class PriceChangedMessage : BaseInboundMessage
    {

        public static readonly string TypeValue = "PriceChanged";

        public string Type { get; set; }

        public string Symbol { get; set; }

        public decimal Price { get; set; }

        public DateTime Timestamp { get; set; }

    }
}
