using System;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Trakx.CryptoCompare.ApiClient.Serialisation.Converters
{
    public class DateTimeOffsetConverter : JsonConverter<DateTimeOffset?>
    {
        public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Debug.Assert(typeToConvert == typeof(DateTimeOffset?));
            var valueRead = reader.GetString();
            if (string.IsNullOrWhiteSpace(valueRead) || valueRead.Equals("null", StringComparison.InvariantCultureIgnoreCase))
                return null;
            return DateTimeOffset.Parse(valueRead);
        }

        public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value?.ToString() ?? "");
        }
    }
}
