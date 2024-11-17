using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MimeDetective.Storage;
#if NET8_0_OR_GREATER
static internal class ByteToHexUtility {
    public const int MaximumLength = 256;
    private static readonly SearchValues<byte> HexSearchValues = SearchValues.Create("abcdefABCDEF0123456789"u8);

    public static byte[] FromHex(ReadOnlySpan<byte> input) {
        Span<char> hexChars = stackalloc char[MaximumLength];

        if (-1 != input.IndexOfAnyExcept(HexSearchValues)) {
            throw new FormatException("Invalid characters in input");
        }

        input = input.Trim((byte)' ');

        Ascii.ToUtf16(input, hexChars, out var written);
        if (input.Length != written) {
            throw new FormatException("Invalid length");
        }

        return Convert.FromHexString(hexChars[..written]);
    }
}
#endif // NET8_0_OR_GREATER

internal abstract class ByteToHexStringConverter<T> : JsonConverter<T>
    where T : IEnumerable<byte> {
    protected static byte[] ReadBytes(ref Utf8JsonReader reader, JsonSerializerOptions options) {
        //var xData = JsonSerializer.Deserialize<string>(ref reader, options)
        //        ?? string.Empty
        //            .Replace(" ", "")
        //    ;
        //return Convert.FromHexString(xData);

#if NET8_0_OR_GREATER
                if (reader.TokenType != JsonTokenType.String) {
                    throw new JsonException("Expected string");
                }

                const int maximumLength = 256;
                Span<byte> buffer = stackalloc byte[maximumLength];
                if (reader.HasValueSequence) {
                    if (reader.ValueSequence.Length > buffer.Length) {
                        throw new FormatException("Input too large");
                    }

                    reader.ValueSequence.CopyTo(buffer);
                    return ByteToHexUtility.FromHex(buffer[..(int)reader.ValueSequence.Length]);
                }

                return ByteToHexUtility.FromHex(reader.ValueSpan);
#else
        var Data = reader.GetString();

        if (Data is null) {
            throw new JsonException("Expected string");
        }

        return Convert.FromHexString(Data);
#endif
    }

    protected static void WriteBytes(Utf8JsonWriter writer, ReadOnlySpan<byte> value, JsonSerializerOptions options) {
        var Data = Convert.ToHexString(
#if NET5_0_OR_GREATER
            value
#else
            value.ToArray()
#endif
        );

#if NET8_0_OR_GREATER
        JsonSerializer.Serialize(writer, Data, MimeDetectiveSourceGeneratedSerializer.Default.String);
#else
        JsonSerializer.Serialize(writer, Data, options);
#endif
        //writer.WriteStringValue(Data);
    }
}
