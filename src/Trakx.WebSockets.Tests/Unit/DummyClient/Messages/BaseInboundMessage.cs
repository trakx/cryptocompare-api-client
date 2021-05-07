using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trakx.WebSockets.Tests.Unit.DummyClient.Messages
{
    public class BaseInboundMessage : IBaseInboundMessage
    {
        public string Type { get; }
    }
}
