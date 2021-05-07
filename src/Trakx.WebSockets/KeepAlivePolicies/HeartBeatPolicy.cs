using System;
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
        private readonly IScheduler? _scheduler;
        private readonly TimeSpan _maxDuration;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly IDateTimeProvider _dateTimeProvider;
        private IDisposable? _subjectSubscription;

        public HeartBeatPolicy(string topicName, TimeSpan maxDuration, 
            IDateTimeProvider dateTimeProvider, IScheduler? scheduler = default)
        {
            TopicName = topicName;
            _maxDuration = maxDuration;
            _dateTimeProvider = dateTimeProvider;
            _scheduler = scheduler ?? Scheduler.Default;
            _cancellationTokenSource = new CancellationTokenSource();
            _lastHeartBeat = dateTimeProvider.UtcNow;
        }

        public string TopicName { get; }

        public bool TryReconnectWhenExceptionHappens => true;
        public void ApplyStrategy<TInboundMessage, TStreamer>(IWebSocketClient<TInboundMessage, TStreamer> client)
            where TInboundMessage : IBaseInboundMessage where TStreamer : IWebSocketStreamer<TInboundMessage>
        {
            client.Streamer.GetStream<TInboundMessage>(TopicName).Subscribe(f => _lastHeartBeat = DateTime.UtcNow);
            StartListening(client);
        }

        public async Task RunHeartBeatCheck<TInboundMessage, TStreamer>(IWebSocketClient<TInboundMessage, TStreamer> client)
            where TInboundMessage : IBaseInboundMessage where TStreamer : IWebSocketStreamer<TInboundMessage>
        {
            if(_lastHeartBeat == null) return;
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
                    await RunHeartBeatCheck(client);
                    return Task.CompletedTask;
                });
            _subjectSubscription = stream.Subscribe(f =>
            {

            });
        }

        public void Dispose()
        {
            _subjectSubscription.Dispose();
        }

    }
}
