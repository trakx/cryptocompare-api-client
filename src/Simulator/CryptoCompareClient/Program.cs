using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Trakx.CryptoCompare.ApiClient;
using Trakx.CryptoCompare.ApiClient.WebSocket;

namespace CryptoCompareWebSocketClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var webSocketClient = new ResilientClientWebsocket();
            var config = new CryptoCompareApiConfiguration
            {
                WebSocketBaseUrl = "ws://localhost:5000",
                ApiKey = "abcdefg"
            };
            var streamer = new WebSocketStreamer();
            var cryptoClient = new Trakx.CryptoCompare.ApiClient.WebSocket.CryptoCompareWebSocketClient(webSocketClient, 
                Options.Create(config), streamer);
            using var sub = cryptoClient.WebSocketStreamer.HeartBeatStream.Subscribe(res =>
            {
                Console.WriteLine($"CryptoCompare Server has triggered HeartBeat event - {res.Message} -- {res.TimeMs}");
            });
            await cryptoClient.Connect();
            Console.ReadKey();
        }
    }
}
