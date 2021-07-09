using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FileDetection.Data.Engine
{
    public abstract class ByteToHexStringConverter<T> : JsonConverter<T>
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

        protected static void WriteBytes(Utf8JsonWriter writer, IEnumerable<byte> value, JsonSerializerOptions options)
        {
            var Data = BitConverter.ToString(value.ToArray()).Replace("-", " ");
            JsonSerializer.Serialize(writer, Data, options);
        }

    }
}
