using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Trakx.CryptoCompare.ApiClient.Serialisation.Converters
{
    /// <summary>
    /// Thanks to https://github.com/dotnet/corefx/issues/41307#issuecomment-562845257
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StringNullableEnumConverter<T> : JsonConverter<T>
    {
        private readonly Type _underlyingType;

        public StringNullableEnumConverter()
        {
            _underlyingType = Nullable.GetUnderlyingType(typeof(T));
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(T).IsAssignableFrom(typeToConvert);
            //return true;
        }

        public override T Read(ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            var value = reader.GetString();
            if (string.IsNullOrEmpty(value)) return default;
            if (!Enum.TryParse(_underlyingType, value, ignoreCase: false, out object result) &&
                !Enum.TryParse(_underlyingType, value, ignoreCase: true, out result))
            {
                throw new JsonException($"Unable to convert \"{value}\" to Enum \"{_underlyingType}\".");
            }
            return (T)result;
        }

        public override void Write(Utf8JsonWriter writer,
            T value,
            JsonSerializerOptions options)
        {
            writer.WriteStringValue(value?.ToString());
        }
    }
}
