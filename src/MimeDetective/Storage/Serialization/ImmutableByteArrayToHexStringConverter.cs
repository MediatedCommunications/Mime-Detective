using System;
using System.Collections.Immutable;
using System.Text.Json;

namespace MimeDetective.Storage;

internal class ImmutableByteArrayToHexStringConverter : ByteToHexStringConverter<ImmutableArray<byte>> {
    public override ImmutableArray<byte> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        var ret = ReadBytes(ref reader, options).ToImmutableArray();
        return ret;
    }

    public override void Write(Utf8JsonWriter writer, ImmutableArray<byte> value, JsonSerializerOptions options) {
        WriteBytes(writer, value.AsSpan(), options);
    }
}
