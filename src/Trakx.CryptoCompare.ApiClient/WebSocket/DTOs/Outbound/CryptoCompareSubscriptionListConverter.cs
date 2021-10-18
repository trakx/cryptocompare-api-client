using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Trakx.CryptoCompare.ApiClient.WebSocket.DTOs.Outbound
{
    public class CryptoCompareSubscriptionListConverter : JsonConverter<IReadOnlyList<ICryptoCompareSubscription>>
    {
        private readonly CryptoCompareSubscriptionConverter _itemConverter;

        public CryptoCompareSubscriptionListConverter()
        {
            _itemConverter = new CryptoCompareSubscriptionConverter();
        }

        #region Overrides of JsonConverter

        /// <inheritdoc />
        public override IReadOnlyList<ICryptoCompareSubscription> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray) { throw new JsonException(); }
            var list = new List<ICryptoCompareSubscription>();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray) return list.AsReadOnly();
                if (reader.TokenType == JsonTokenType.String) list.Add(_itemConverter.Read(ref reader, typeof(ICryptoCompareSubscription), options));
            }
            throw new JsonException();
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, IReadOnlyList<ICryptoCompareSubscription> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (var cryptoCompareSubscription in value)
            {
                _itemConverter.Write(writer, cryptoCompareSubscription, options);
            }
            writer.WriteEndArray();
        }

        #endregion
    }
}