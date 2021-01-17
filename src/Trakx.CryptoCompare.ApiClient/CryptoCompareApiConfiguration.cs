using System;
using System.Text.Json.Serialization;
using Trakx.Utils.Attributes;

namespace Trakx.CryptoCompare.ApiClient
{
    public record CryptoCompareApiConfiguration
    {
#nullable disable
        public string RestBaseUrl { get; init; }
        public string WebSocketBaseUrl { get; init; } = "wss://streamer.cryptocompare.com/";
        
        [SecretEnvironmentVariable(nameof(ApiKey))]
        public string ApiKey { get; set; }
         
        [JsonIgnore] 
        public Uri WebSocketEndpoint => new Uri(new Uri(WebSocketBaseUrl), $"v2?api_key={ApiKey}");

        public int ThrottleDelayMs { get; init; } = 0;
#nullable restore
    }
}
 