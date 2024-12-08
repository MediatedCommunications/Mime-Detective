#if NET7_0_OR_GREATER
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MimeDetective.Storage;

[JsonSourceGenerationOptions(
    AllowTrailingCommas = true,
    PropertyNameCaseInsensitive = true,
    WriteIndented = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
    UseStringEnumConverter = true,
    Converters = [typeof(ImmutableByteArrayToHexStringConverter)]
)]
[JsonSerializable(typeof(Definition[]))]
[JsonSerializable(typeof(IEnumerable<Definition>))]
internal partial class MimeDetectiveSourceGeneratedSerializer : JsonSerializerContext;

#endif // NET7_0_OR_GREATER
