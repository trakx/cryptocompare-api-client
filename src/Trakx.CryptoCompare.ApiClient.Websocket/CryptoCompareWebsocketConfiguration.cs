using Trakx.Utils.Attributes;

#pragma warning disable 8618
namespace Trakx.CryptoCompare.ApiClient.Websocket
{
    public record CryptoCompareWebsocketConfiguration
    {
        public string Url { get; init; }

        [SecretEnvironmentVariable]
        public string ApiKey { get; init; }
    }
}