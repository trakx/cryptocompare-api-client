using System;
using Newtonsoft.Json;
using Trakx.CryptoCompare.ApiClient.Rest.Extensions;

namespace Trakx.CryptoCompare.ApiClient.Rest.Converters
{
    internal class UnixTimeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime) || objectType == typeof(DateTime?)
                                                  || objectType == typeof(DateTimeOffset)
                                                  || objectType == typeof(DateTimeOffset?);
        }

        public override object? ReadJson(
            JsonReader reader,
            Type objectType,
            object? existingValue,
            JsonSerializer serializer)
        {
            return string.IsNullOrWhiteSpace(reader.Value?.ToString())
                ? (object?) null
                : Convert.ToInt64(reader.Value).FromUnixTime();
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (DateTimeOffset.TryParse(value?.ToString(), out var date))
            {
                writer.WriteValue(date.ToUnixTime());
            }
        }
    }
}
