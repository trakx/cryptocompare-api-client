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
                await ReceiveAndSend(webSocket, (result, buffer) =>
                {
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
#if DEBUG
                        Console.WriteLine($"Receive->Text");
                        Console.WriteLine($"Message: {Encoding.UTF8.GetString(buffer, 0, result.Count)}");
#endif
                        return;
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
#if DEBUG
                        Console.WriteLine($"Receive->Close");
#endif

                        return;
                    }
                });
            }
            else
            {
                await _next(context);
            }
        }

        private async Task ReceiveAndSend(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
        {
            while (socket.State == WebSocketState.Open)
            {
                var data = $"{{ \"TYPE\": \"999\", \"MESSAGE\": \"AAA\", \"TIMEMS\": {DateTime.Now.Second} }}";
                var buffer = Encoding.UTF8.GetBytes(data);
                await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                await Task.Delay(TimeSpan.FromSeconds(3));
            }
        }
    }
}