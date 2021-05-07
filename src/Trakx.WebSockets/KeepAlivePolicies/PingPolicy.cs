using System;
using System.Net.WebSockets;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Trakx.WebSockets.KeepAlivePolicies
{
    public class PingPolicy : IKeepAlivePolicy, IDisposable
    {

        private readonly IScheduler? _scheduler;
        private readonly TimeSpan _keepAliveInterval;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly string _pingMessage;
        private IDisposable? _subjectSubscription;

        public PingPolicy(TimeSpan keepAliveInterval, 
            string pingMessage = "ping",
            IScheduler? scheduler = default)
        {
            _keepAliveInterval = keepAliveInterval;
            _pingMessage = pingMessage;
            _scheduler = scheduler ?? Scheduler.Default;
            _cancellationTokenSource = new CancellationTokenSource();
        }


        public bool TryReconnectWhenExceptionHappens => true;

        public void ApplyStrategy<TInboundMessage, TStreamer>(IWebSocketClient<TInboundMessage, TStreamer> client)
            where TInboundMessage : IBaseInboundMessage where TStreamer : IWebSocketStreamer<TInboundMessage>
        {
            StartPinging(client);
        }

        public async Task PingServer<TInboundMessage, TStreamer>(IWebSocketClient<TInboundMessage, TStreamer> client)
            where TInboundMessage : IBaseInboundMessage where TStreamer : IWebSocketStreamer<TInboundMessage>
        {
            var messageBytes = Encoding.UTF8.GetBytes(_pingMessage).AsMemory();
            await client.WebSocket.SendAsync(messageBytes, WebSocketMessageType.Text, true, 
                _cancellationTokenSource.Token);
        }

        private void StartPinging<TInboundMessage, TStreamer>(IWebSocketClient<TInboundMessage, TStreamer> client)
            where TInboundMessage : IBaseInboundMessage where TStreamer : IWebSocketStreamer<TInboundMessage>
        {
            var stream = Observable.Interval(_keepAliveInterval, _scheduler!)
                .TakeUntil(_ => _cancellationTokenSource.Token.IsCancellationRequested)
                .SelectMany(async _ =>
                {
                    await PingServer(client).ConfigureAwait(false);
                    return Task.CompletedTask;
                });
            _subjectSubscription = stream.Subscribe(_ => { });
        }

        public void Dispose()
        {
            _subjectSubscription?.Dispose();
        }

    }
}
