using System;
using System.Text.Json.Serialization;
using Trakx.Common.Attributes;

namespace Trakx.CryptoCompare.ApiClient;

public record CryptoCompareApiConfiguration
{
    public string WebSocketBaseUrl { get; init; } = "wss://streamer.cryptocompare.com/";

    [AwsParameter(AllowGlobal = true)]
    public string? ApiKey { get; set; }

    [JsonIgnore]
    public Uri WebSocketEndpoint => new(new Uri(WebSocketBaseUrl), $"v2?api_key={ApiKey}");

    public int ThrottleDelayMs { get; init; } = 0;
}
