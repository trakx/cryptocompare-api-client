using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Trakx.CryptoCompare.ApiClient.Serialisation.Converters
{
    /// <summary>
    /// Credits to https://github.com/dotnet/runtime/issues/30524#issuecomment-524619972
    /// </summary>
    public class JsonNonStringKeyDictionaryConverter<TKey, TValue> : JsonConverter<IDictionary<TKey, TValue>> where TKey : notnull
    {
        public override IDictionary<TKey, TValue> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var convertedType = typeof(Dictionary<,>)
                .MakeGenericType(typeof(string), typeToConvert.GenericTypeArguments[1]);
            var value = JsonSerializer.Deserialize(ref reader, convertedType, options);
            var instance = (Dictionary<TKey, TValue>)Activator.CreateInstance(
                typeof(Dictionary<TKey, TValue>),
                BindingFlags.Instance | BindingFlags.Public,
                null,
                null,
                CultureInfo.CurrentCulture)!;
            var enumerator = (IEnumerator)convertedType.GetMethod("GetEnumerator")!.Invoke(value, null)!;
            var parse = typeof(TKey).GetMethod("Parse", 0, BindingFlags.Public | BindingFlags.Static, null, CallingConventions.Any, new[] { typeof(string) }, null);
            if (parse == null) throw new NotSupportedException($"{typeof(TKey)} as TKey in IDictionary<TKey, TValue> is not supported.");
            while (enumerator.MoveNext())
            {
                var element = (KeyValuePair<string, TValue>)enumerator.Current!;
                instance.Add((TKey)parse.Invoke(null, new [] { element.Key })!, element.Value);
            }
            return instance;
        }

        public override void Write(Utf8JsonWriter writer, IDictionary<TKey, TValue> value, JsonSerializerOptions options)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            var convertedDictionary = new Dictionary<string, TValue>(value.Count);
            foreach (var (k, v) in value) if(k != null) convertedDictionary[k.ToString()!] = v;

            JsonSerializer.Serialize(writer, convertedDictionary, options);
            convertedDictionary.Clear();
        }
    }

    public sealed class JsonDateTimeKeyDictionaryConverter<TValue> : JsonNonStringKeyDictionaryConverter<DateTime, TValue>
    {
        public override void Write(Utf8JsonWriter writer, IDictionary<DateTime, TValue> value, JsonSerializerOptions options)
        {
            var convertedDictionary = new Dictionary<string, TValue>(value.Count);
            foreach (var (k, v) in value) convertedDictionary[k.ToString("yyyy-MM-ddTHH:mm:ssK")] = v;

            JsonSerializer.Serialize(writer, convertedDictionary, options);
            convertedDictionary.Clear();
        }
    }
}