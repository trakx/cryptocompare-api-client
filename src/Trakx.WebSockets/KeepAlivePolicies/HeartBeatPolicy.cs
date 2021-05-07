using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Namotion.Reflection;
using Trakx.Utils.DateTimeHelpers;

namespace Trakx.WebSockets.KeepAlivePolicies
{
    public class HeartBeatPolicy : IKeepAlivePolicy, IDisposable
    {

        private DateTime? _lastHeartBeat;
        private readonly IObserver<Task> _subscription;
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

        private void StartListening<TInboundMessage, TStreamer>(IWebSocketClient<TInboundMessage, TStreamer> client)
            where TInboundMessage : IBaseInboundMessage where TStreamer : IWebSocketStreamer<TInboundMessage>
        {
            var stream = Observable.Interval(_maxDuration, _scheduler!)
                .TakeUntil(_ => _cancellationTokenSource.Token.IsCancellationRequested)
                .SelectMany(async _ =>
                {
                    var duration = _dateTimeProvider.UtcNow - _lastHeartBeat.Value;
                    if (duration > _maxDuration)
                    {
                        await client.WebSocket.RecycleConnectionAsync(_cancellationTokenSource.Token);
                    }
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
