using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MimeDetective.Storage;

#if NET8_0_OR_GREATER
internal static class ByteToHexUtility {
    public const int MaximumStackalloc = 512;
    public const int MaximumLength = 8 * 1024;
    private static readonly SearchValues<byte> HexSearchValues = SearchValues.Create("abcdefABCDEF0123456789"u8);

    public static byte[] FromHex(ReadOnlySpan<byte> input) {
        if (input.Length > MaximumLength) {
            throw new FormatException("Input too long");
        }

        if (-1 != input.IndexOfAnyExcept(HexSearchValues)) {
            throw new FormatException("Invalid characters in input");
        }

        input = input.Trim((byte)' ');

        char[]? poolBuffer = null;
        var hexChars = input.Length <= MaximumStackalloc
            ? stackalloc char[MaximumStackalloc]
            : (poolBuffer = ArrayPool<char>.Shared.Rent(input.Length)).AsSpan();

        try {
            Ascii.ToUtf16(input, hexChars, out var written);
            if (input.Length != written) {
                throw new FormatException("Invalid length");
            }

            return Convert.FromHexString(hexChars[..written]);
        } finally {
            if (poolBuffer is not null) {
                ArrayPool<char>.Shared.Return(poolBuffer);
            }
        }
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

        if (!reader.HasValueSequence) {
            return ByteToHexUtility.FromHex(reader.ValueSpan);
        }

        if (reader.ValueSequence.Length > ByteToHexUtility.MaximumLength) {
            throw new JsonException("Input too long");
        }

        byte[]? poolBuffer = null;
        try {
            var buffer = reader.ValueSequence.Length <= ByteToHexUtility.MaximumStackalloc
                ? stackalloc byte[ByteToHexUtility.MaximumStackalloc]
                : (poolBuffer = ArrayPool<byte>.Shared.Rent((int)reader.ValueSequence.Length)).AsSpan();

            reader.ValueSequence.CopyTo(buffer);

            return ByteToHexUtility.FromHex(buffer[..(int)reader.ValueSequence.Length]);
        } finally {
            if (poolBuffer is not null) {
                ArrayPool<byte>.Shared.Return(poolBuffer);
            }
        }
#else
        var data = reader.GetString()
            ?? throw new JsonException("Expected string");

        return Convert.FromHexString(data);
#endif
    }

    protected static void WriteBytes(Utf8JsonWriter writer, ReadOnlySpan<byte> value, JsonSerializerOptions options) {
        var data = Convert.ToHexString(
#if NET5_0_OR_GREATER
            value
#else
            value.ToArray()
#endif
        );

#if NET8_0_OR_GREATER
        JsonSerializer.Serialize(writer, data, MimeDetectiveSourceGeneratedSerializer.Default.String);
#else
        JsonSerializer.Serialize(writer, data, options);
#endif
        //writer.WriteStringValue(Data);
    }
}
