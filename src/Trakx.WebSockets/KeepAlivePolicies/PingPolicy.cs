using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Trakx.Utils.DateTimeHelpers;

namespace Trakx.WebSockets.KeepAlivePolicies
{
    public class PingPolicy : IKeepAlivePolicy, IDisposable
    {

        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IScheduler _scheduler;
        private readonly TimeSpan _pingInterval;
        private readonly string _pingMessage;
        private readonly CancellationTokenSource _cancellationTokenSource;

        private DateTime? _lastPingDateTime;
        private IDisposable? _subjectSubscription;

        public PingPolicy(TimeSpan pingInterval, 
            string pingMessage = "ping",
            IDateTimeProvider? dateTimeProvider = default,
            IScheduler? scheduler = default)
        {
            _pingInterval = pingInterval;
            _pingMessage = pingMessage;
            _dateTimeProvider = dateTimeProvider ?? new DateTimeProvider();
            _scheduler = scheduler ?? Scheduler.Default;
            _cancellationTokenSource = new CancellationTokenSource();
        }


        public bool TryReconnectWhenWebSocketErrors => true;

        public DateTime? LastPingDateTime => _lastPingDateTime;

        public TimeSpan PingInterval => _pingInterval;

        public string PingMessage => _pingMessage;

        public virtual void Apply<TInboundMessage, TStreamer>(IWebSocketClient<TInboundMessage, TStreamer> client)
            where TInboundMessage : IBaseInboundMessage where TStreamer : IWebSocketStreamer<TInboundMessage>
        {
            StartPinging(client);
        }

        private void StartPinging<TInboundMessage, TStreamer>(IWebSocketClient<TInboundMessage, TStreamer> client)
            where TInboundMessage : IBaseInboundMessage where TStreamer : IWebSocketStreamer<TInboundMessage>
        {
            client.WebSocket.PingServer(PingMessage, _cancellationTokenSource.Token).GetAwaiter().GetResult();
            var stream = Observable.Interval(PingInterval, _scheduler!)
                .TakeUntil(_ => _cancellationTokenSource.Token.IsCancellationRequested)
                .SelectMany(async _ =>
                {
                    _lastPingDateTime = _dateTimeProvider.UtcNow;
                    await client.WebSocket.PingServer(PingMessage, _cancellationTokenSource.Token).ConfigureAwait(false);
                    return Task.CompletedTask;
                });
            _subjectSubscription = stream.Subscribe(_ => { });
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            _cancellationTokenSource.Dispose();
            _subjectSubscription?.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
