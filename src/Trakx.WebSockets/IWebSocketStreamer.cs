using System;
using System.Collections.Generic;
using System.Reactive.Subjects;

namespace Trakx.WebSockets
{
    public interface IWebSocketStreamer<TBaseMessage>
    {

        /// <summary>
        /// Returns the class type of one specific inbound message name.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        Type? GetMessageType(string typeName);

        /// <summary>
        /// Returns the main stream of the messages.
        /// </summary>
        ISubject<TBaseMessage> InboundMessages { get; }

        /// <summary>
        /// Returns the complete list of streams, but in dictionary format.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, IObservable<TBaseMessage>> GetStreams();

        /// <summary>
        /// Return one specific stream based on its message type.
        /// </summary>
        /// <typeparam name="TMessageType"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        IObservable<TMessageType> GetStream<TMessageType>(string name);

        /// <summary>
        /// Publish one message from websocket to streamer.
        /// </summary>
        /// <param name="rawMessage"></param>
        void PublishInboundMessageOnStream(string rawMessage);

    }
}
