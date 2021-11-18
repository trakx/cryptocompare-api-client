using Trakx.Utils.Attributes;

namespace Trakx.CryptoCompare.ApiClient.Websocket
{
    public class CryptoCompareWebsocketConfiguration
    {
        public string Url { get; set; }

        [SecretEnvironmentVariable]
        public string ApiKey { get; set; }
    }
}