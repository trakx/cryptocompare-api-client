using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CryptoCompareServer.Middleware
{
    public class CryptoCompareServerMiddleware
    {

        private readonly RequestDelegate _next;

        public CryptoCompareServerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();

#if DEBUG
                Console.WriteLine("WebSocket Connected");
#endif
                await Send(webSocket);
            }
            else
            {
                await _next(context);
            }
        }

        private async Task Send(WebSocket socket)
        {
            while (socket.State == WebSocketState.Open)
            {
                var data = $"{{ \"TYPE\": \"999\", \"MESSAGE\": \"AAA\", \"TIMEMS\": {DateTime.Now.Second} }}";
                var buffer = Encoding.UTF8.GetBytes(data);
                await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                await Task.Delay(TimeSpan.FromSeconds(3)).ConfigureAwait(false);
            }
        }
    }
}