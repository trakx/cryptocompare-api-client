using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Reflection;
using System.Text.Json;
using Serilog;

namespace Trakx.WebSockets
{
    public abstract class WebSocketStreamer<TBaseMessage> : IWebSocketStreamer<TBaseMessage>
        where TBaseMessage : IBaseInboundMessage
    {

        private readonly ILogger _logger = Log.Logger.ForContext(MethodBase.GetCurrentMethod()!.DeclaringType);


        protected WebSocketStreamer()
        {
            InboundMessages = new ReplaySubject<TBaseMessage>(1);
        }

        public void PublishInboundMessageOnStream(string rawMessage)
        {
            try
            {
                _logger.Verbose("Received WebSocketInboundMessage {0}{1}", Environment.NewLine, rawMessage);
                var message = JsonSerializer.Deserialize<TBaseMessage>(rawMessage);
                var type = GetMessageType(message!.Type);
                if (type == null) return;
                InboundMessages.OnNext((TBaseMessage)JsonSerializer.Deserialize(rawMessage, type)!);
            }
            catch (Exception exception)
            {
                _logger.Error(exception, "Failed to publish {0}", rawMessage);
            }
        }

        public ISubject<TBaseMessage> InboundMessages { get; }

        public abstract Type GetMessageType(string typeName);

        public abstract Dictionary<string, IObservable<TBaseMessage>> GetStreams();

        public abstract IObservable<TMessageType> GetStream<TMessageType>(string topicName);

    }
}
