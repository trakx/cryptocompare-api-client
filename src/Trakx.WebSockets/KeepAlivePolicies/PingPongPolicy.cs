﻿using System;
using System.Linq;
using System.Net.WebSockets;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Trakx.Utils.DateTimeHelpers;

namespace Trakx.WebSockets.KeepAlivePolicies
{
    public class PingPongPolicy : IKeepAlivePolicy, IDisposable
    {

        private readonly IScheduler _scheduler;
        private readonly TimeSpan _pongTimeout;
        private readonly string _pingMessage;
        private readonly string _expectedPongMessage;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly IDateTimeProvider _dateTimeProvider;
        private IDisposable? _subscription;
        private DateTime? _lastPongDateTime;

        public PingPongPolicy(string pingMessage,
            TimeSpan pongTimeout,
            string expectedPongMessage,
            IDateTimeProvider? dateTimeProvider = default,
            IScheduler? scheduler = default)
        {
            _pingMessage = pingMessage;
            _pongTimeout = pongTimeout;
            _expectedPongMessage = expectedPongMessage;
            _dateTimeProvider = dateTimeProvider ?? new DateTimeProvider();
            _scheduler = scheduler ?? Scheduler.Default;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public DateTime? LastPongDateTime => _lastPongDateTime;

        public TimeSpan PongTimeout => _pongTimeout;

        public string PingMessage => _pingMessage;

        public string ExpectedPongMessage => _expectedPongMessage;

        public bool TryReconnectWhenWebSocketErrors => true;

        public void Apply<TInboundMessage, TStreamer>(IWebSocketClient<TInboundMessage, TStreamer> client)
            where TInboundMessage : IBaseInboundMessage where TStreamer : IWebSocketStreamer<TInboundMessage>
        {
            AwaitingPong(client);
        }

        private void AwaitingPong<TInboundMessage, TStreamer>(IWebSocketClient<TInboundMessage, TStreamer> client)
            where TInboundMessage : IBaseInboundMessage where TStreamer : IWebSocketStreamer<TInboundMessage>
        {
            _lastPongDateTime = _dateTimeProvider.UtcNow;
            client.WebSocket.PingServer(_pingMessage, _cancellationTokenSource.Token);
            var stream = Observable.Interval(PongTimeout / 3, _scheduler!)
                .TakeUntil(_ => _cancellationTokenSource.Token.IsCancellationRequested)
                .SelectMany(async _ =>
                {
                    while (client.WebSocket.State == WebSocketState.Open &&
                           !_cancellationTokenSource.IsCancellationRequested)
                    {
                        var buffer = new ArraySegment<byte>(new byte[4096]);
                        var receiveResult = await client.WebSocket.ReceiveAsync(buffer, _cancellationTokenSource.Token).ConfigureAwait(false);
                        if (receiveResult.MessageType == WebSocketMessageType.Close) break;
                        var msgBytes = buffer.Skip(buffer.Offset).Take(receiveResult.Count).ToArray();
                        var result = Encoding.UTF8.GetString(msgBytes);

                        if (result == ExpectedPongMessage)
                        {
                            _lastPongDateTime = _dateTimeProvider.UtcNow;
                            await client.WebSocket.PingServer(PingMessage, _cancellationTokenSource.Token);
                        }

                        if (_dateTimeProvider.UtcNow - LastPongDateTime >= PongTimeout)
                        {
                            await client.WebSocket.RecycleConnectionAsync(_cancellationTokenSource.Token);
                        }
                    }
                    return Task.CompletedTask;
                });
            _subscription = stream.Subscribe(_ => { });
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            _cancellationTokenSource.Dispose();
            _subscription?.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
