using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Trakx.Utils.DateTimeHelpers;

namespace Trakx.WebSockets.KeepAlivePolicies
{
    public class HeartBeatPolicy : IKeepAlivePolicy, IDisposable
    {
        private DateTime? _lastHeartBeat;
        private readonly IScheduler _scheduler;
        private readonly TimeSpan _maxDuration;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly IDateTimeProvider _dateTimeProvider;
        private IDisposable? _subscription;

        public HeartBeatPolicy(string streamName, TimeSpan maxDuration,
            IDateTimeProvider dateTimeProvider, IScheduler? scheduler = default)
        {
            StreamName = streamName;
            _maxDuration = maxDuration;
            _dateTimeProvider = dateTimeProvider;
            _scheduler = scheduler ?? Scheduler.Default;
            _cancellationTokenSource = new CancellationTokenSource();
            _lastHeartBeat = dateTimeProvider.UtcNow;
        }

        public string StreamName { get; }

        public bool TryReconnectWhenWebSocketErrors => true;

        public void Apply<TInboundMessage, TStreamer>(IWebSocketClient<TInboundMessage, TStreamer> client)
            where TInboundMessage : IBaseInboundMessage where TStreamer : IWebSocketStreamer<TInboundMessage>
        {
            var stream = client.Streamer.GetStream<TInboundMessage>(StreamName);
            if (stream == null) throw new KeyNotFoundException(nameof(StreamName));
            stream.Subscribe(_ => _lastHeartBeat = DateTime.UtcNow);
            StartListening(client);
        }

        public async Task RunHeartBeatCheck<TInboundMessage, TStreamer>(IWebSocketClient<TInboundMessage, TStreamer> client)
            where TInboundMessage : IBaseInboundMessage where TStreamer : IWebSocketStreamer<TInboundMessage>
        {
            if (_lastHeartBeat == null) return;
            var duration = _dateTimeProvider.UtcNow - _lastHeartBeat.Value;
            if (duration > _maxDuration)
            {
                await client.WebSocket.RecycleConnectionAsync(_cancellationTokenSource.Token);
            }
        }

        private void StartListening<TInboundMessage, TStreamer>(IWebSocketClient<TInboundMessage, TStreamer> client)
            where TInboundMessage : IBaseInboundMessage where TStreamer : IWebSocketStreamer<TInboundMessage>
        {
            var stream = Observable.Interval(_maxDuration, _scheduler!)
                .TakeUntil(_ => _cancellationTokenSource.Token.IsCancellationRequested)
                .SelectMany(async _ =>
                {
                    await RunHeartBeatCheck(client).ConfigureAwait(false);
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
