using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MimeDetective.Storage {
    internal abstract class ByteToHexStringConverter<T> : JsonConverter<T>
       where T : IEnumerable<byte>
    {
        protected static byte[] ReadBytes(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            var Data = JsonSerializer.Deserialize<string>(ref reader, options)
                ?? string.Empty
                .Replace(" ", "")
                ;

            return Convert.FromHexString(Data);

        }

        protected static void WriteBytes(Utf8JsonWriter writer, ReadOnlySpan<byte> value, JsonSerializerOptions options) {
            var Data = Convert.ToHexString(
#if NET5_0_OR_GREATER
                value
#else
                value.ToArray()
#endif
            );

            JsonSerializer.Serialize(writer, Data, options);
        }

    }
}
