using System;
using System.Text.Json.Serialization;

namespace Trakx.CryptoCompare.ApiClient
{
    public class CryptoCompareApiConfiguration
    {
#nullable disable
        public string RestBaseUrl { get; set; }
        public string WebSocketBaseUrl { get; set; } = "wss://streamer.cryptocompare.com/";
        
        public string ApiKey { get; set; }
        //public int? MaxRetryCount { get; set; }
         
        [JsonIgnore] public Uri WebSocketEndpoint => new Uri(new Uri(WebSocketBaseUrl), $"v2?api_key={ApiKey}");

        public int ThrottleDelayMs { get; set; } = 0;
#nullable restore
    }
}
 