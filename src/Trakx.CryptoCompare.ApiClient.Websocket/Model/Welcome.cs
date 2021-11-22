using System.Text.Json.Serialization;

namespace Trakx.CryptoCompare.ApiClient.Websocket.Model
{
    public class Welcome : InboundMessageBase
    {
#nullable disable

        [JsonPropertyName("MESSAGE")]
        public string Message { get; set; }

        [JsonPropertyName("SERVER_UPTIME_SECONDS")]
        public int ServerUptimeSeconds { get; set; }

        [JsonPropertyName("SERVER_NAME")]
        public string ServerName { get; set; }

        [JsonPropertyName("SERVER_TIME_MS")]
        public long ServerTimeMs { get; set; }

        [JsonPropertyName("CLIENT_ID")]
        public int ClientId { get; set; }

        [JsonPropertyName("DATA_FORMAT")]
        public string DataFormat { get; set; }

        [JsonPropertyName("SOCKETS_ACTIVE")]
        public int SocketsActive { get; set; }

        [JsonPropertyName("SOCKETS_REMAINING")]
        public int SocketsRemaining { get; set; }

        [JsonPropertyName("RATELIMIT_MAX_SECOND")]
        public int RateLimitMaxSecond { get; set; }

        [JsonPropertyName("RATELIMIT_MAX_MINUTE")]
        public int RateLimitMaxMinute { get; set; }

        [JsonPropertyName("RATELIMIT_MAX_HOUR")]
        public int RateLimitMaxHour { get; set; }

        [JsonPropertyName("RATELIMIT_MAX_DAY")]
        public int RateLimitMaxDay { get; set; }

        [JsonPropertyName("RATELIMIT_MAX_MONTH")]
        public int RateLimitMaxMonth { get; set; }

        [JsonPropertyName("RATELIMIT_REMAINING_SECOND")]
        public int RateLimitRemainingSecond { get; set; }

        [JsonPropertyName("RATELIMIT_REMAINING_MINUTE")]
        public int RateLimitRemainingMinute { get; set; }

        [JsonPropertyName("RATELIMIT_REMAINING_HOUR")]
        public int RateLimitRemainingHour { get; set; }

        [JsonPropertyName("RATELIMIT_REMAINING_DAY")]
        public int RateLimitRemainingDay { get; set; }

        [JsonPropertyName("RATELIMIT_REMAINING_MONTH")]
        public int RateLimitRemainingMonth { get; set; }
#nullable restore
    }
}
