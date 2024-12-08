using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MimeDetective.Storage;

/// <summary>
///     Read and write <see cref="Definition" />s in raw JSON form.
/// </summary>
public static class DefinitionJsonSerializer {
    private static JsonSerializerOptions SerializerOptions() {
        var ret = new JsonSerializerOptions {
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            Converters = {
                new ImmutableByteArrayToHexStringConverter(),
#if NET7_0_OR_GREATER
            },
            TypeInfoResolver = MimeDetectiveSourceGeneratedSerializer.Default
#else
                new JsonStringEnumConverter()
            }
#endif
        };

        return ret;
    }

    public static Definition[] FromJson(Stream content) {
        var ret =
#if NET7_0_OR_GREATER
            JsonSerializer.Deserialize(content, MimeDetectiveSourceGeneratedSerializer.Default.DefinitionArray);
#else
            JsonSerializer.Deserialize<Definition[]>(content, SerializerOptions());
#endif
        return ret ?? [];
    }

#if NET8_0_OR_GREATER
    [RequiresDynamicCode("The JSON deserializer may require dynamic code")]
    [RequiresUnreferencedCode("JSON deserialization may require types that cannot be statically analyzed.")]
#endif
    public static Definition[] FromJson(JsonSerializerOptions? options, Stream content) {
        if (options is null) {
            return FromJson(content);
        }

        var ret = JsonSerializer.Deserialize<Definition[]>(content, options) ?? [];

        return ret;
    }


    public static void ToJson(Stream data, IEnumerable<Definition> values) {
#if NET7_0_OR_GREATER
        JsonSerializer.Serialize(data, values, MimeDetectiveSourceGeneratedSerializer.Default.IEnumerableDefinition);
#else
        JsonSerializer.Serialize(data, values, SerializerOptions());
#endif
    }

#if NET7_0_OR_GREATER
    [RequiresDynamicCode("JSON serialization may require dynamic code.")]
    [RequiresUnreferencedCode("JSON serialization may require types that cannot be statically analyzed.")]
#endif
    public static void ToJson(JsonSerializerOptions? options, Stream data, IEnumerable<Definition> values) {
        if (options is null) {
            ToJson(data, values);
        } else {
            JsonSerializer.Serialize(data, values, options);
        }
    }

#if NET7_0_OR_GREATER
    [RequiresDynamicCode("JSON serialization may require dynamic code.")]
    [RequiresUnreferencedCode("JSON serialization may require types that cannot be statically analyzed.")]
#endif
    public static void ToJsonFile(string fileName, IEnumerable<Definition> values, JsonSerializerOptions? options = default) {
        using var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read, 64 * 1024);

        if (options is null) {
            ToJson(stream, values);
        } else {
            JsonSerializer.Serialize(stream, values, options);
        }
    }
}
