using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Trakx.CryptoCompare.ApiClient.WebSocket.DTOs.Outbound
{
    public class CryptoCompareSubscriptionConverter : JsonConverter<ICryptoCompareSubscription>
    {
        public override ICryptoCompareSubscription Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String) throw new FormatException($"{reader.TokenType} should be a string to be read as a CryptoCompareSubscription");
            var subscriptionString = reader.GetString();
            return ParseSubscriptionString(subscriptionString);
        }

        public static ICryptoCompareSubscription ParseSubscriptionString(string subscriptionString)
        {
            var split = subscriptionString.Split("~");
            switch (split[0])
            {
                case TradeSubscription.TypeValue:
                    return new TradeSubscription(split[1], split[2], split[3]);
                case TickerSubscription.TypeValue:
                    return new TickerSubscription(split[1], split[2], split[3]);
                case AggregateIndexSubscription.TypeValue:
                    return new AggregateIndexSubscription(split[2], split[3]);
                //todo: case OrderBookL2
                case FullVolumeSubscription.TypeValue:
                    return new FullVolumeSubscription(split[1]);
                case FullTopTierVolumeSubscription.TypeValue:
                    return new FullTopTierVolumeSubscription(split[1]);
                case OhlcSubscription.TypeValue:
                    var timespan = split.Length < 5
                        ? default
                        : split[4] == "D"
                            ? TimeSpan.FromDays(1)
                            : split[4] == "H"
                                ? TimeSpan.FromHours(1)
                                : TimeSpan.FromMinutes(1);
                    return new OhlcSubscription(split[1], split[2], split[3], timespan);
                default:
                    throw new InvalidDataException($"Failed to parse {subscriptionString} as a subscription");
            }
        }

        public override void Write(Utf8JsonWriter writer, ICryptoCompareSubscription value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
