using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MimeDetective.Storage;

/// <summary>
/// Read and write <see cref="Definition"/>s in raw JSON form.
/// </summary>
public static partial class DefinitionJsonSerializer {
    private static JsonSerializerOptions SerializerOptions() {
        var ret = new JsonSerializerOptions() {
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault,
            Converters =
            {
                new ImmutableByteArrayToHexStringConverter(),
#if NET7_0_OR_GREATER
                },
            TypeInfoResolver = MimeDetectiveSourceGeneratedSerializer.Default
#else
                new JsonStringEnumConverter(),
            }
#endif
        };

        return ret;
    }

    public static Definition[] FromJson(Stream Content) {
        var ret =
#if NET7_0_OR_GREATER
            JsonSerializer.Deserialize(Content, MimeDetectiveSourceGeneratedSerializer.Default.DefinitionArray);
#else
            JsonSerializer.Deserialize<Definition[]>(Content, SerializerOptions());
#endif
        return ret ?? [];
    }

#if NET8_0_OR_GREATER
    [RequiresDynamicCodeAttribute("The JSON deserializer may require dynamic code")]
    [RequiresUnreferencedCode("JSON deserialization may require types that cannot be statically analyzed.")]
#endif
    public static Definition[] FromJson(JsonSerializerOptions? Options, Stream Content) {
        if (Options is null) {
            return FromJson(Content);
        }

        var ret = JsonSerializer.Deserialize<Definition[]>(Content, Options) ?? [];

        return ret;
    }


    public static void ToJson(Stream Data, IEnumerable<Definition> Values) {
#if NET7_0_OR_GREATER
        JsonSerializer.Serialize(Data, Values, MimeDetectiveSourceGeneratedSerializer.Default.IEnumerableDefinition);
#else
        JsonSerializer.Serialize(Data, Values, SerializerOptions());
#endif
    }

#if NET7_0_OR_GREATER
    [RequiresDynamicCode("JSON serialization may require dynamic code.")]
    [RequiresUnreferencedCode("JSON serialization may require types that cannot be statically analyzed.")]
#endif
    public static void ToJson(JsonSerializerOptions? Options, Stream Data, IEnumerable<Definition> Values) {
        if (Options is null) {
            ToJson(Data, Values);
        } else {
            JsonSerializer.Serialize(Data, Values, Options);
        }
    }

#if NET7_0_OR_GREATER
    [RequiresDynamicCode("JSON serialization may require dynamic code.")]
    [RequiresUnreferencedCode("JSON serialization may require types that cannot be statically analyzed.")]
#endif
    public static void ToJsonFile(string FileName, IEnumerable<Definition> Values, JsonSerializerOptions? Options = default) {
        using var Stream = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.Read, 64 * 1024);

        if (Options is null) {
            ToJson(Stream, Values);
        } else {
            JsonSerializer.Serialize(Stream, Values, Options);
        }
    }

}