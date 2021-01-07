using System;
using System.Text.Json.Serialization;
using Trakx.Utils.Attributes;

namespace Trakx.CryptoCompare.ApiClient
{
    public class CryptoCompareApiConfiguration
    {
#nullable disable
        public string RestBaseUrl { get; set; }
        public string WebSocketBaseUrl { get; set; } = "wss://streamer.cryptocompare.com/";
        
        [ReadmeDocument("CryptoCompareApiConfiguration__ApiKey")]
        public string ApiKey { get; set; }
         
        [JsonIgnore] public Uri WebSocketEndpoint => new Uri(new Uri(WebSocketBaseUrl), $"v2?api_key={ApiKey}");

        public int ThrottleDelayMs { get; set; } = 0;
#nullable restore
    }
}
 