using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MimeDetective.Storage;

/// <summary>
///     Read and write <see cref="Definition" />s in a compressed binary form.
/// </summary>
public static class DefinitionBinarySerializer {
    private static JsonSerializerOptions SerializerOptions() {
        var ret = new JsonSerializerOptions {
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = true,
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
#if NET7_0_OR_GREATER
            TypeInfoResolver = MimeDetectiveSourceGeneratedSerializer.Default
#endif
        };

        return ret;
    }


    public static Definition[] FromBinary(Stream data) {
        using var cs = new GZipStream(data, CompressionMode.Decompress, true);
        using var bs = new BufferedStream(cs, 128 * 1024);

        var ret = DefinitionJsonSerializer.FromJson(bs);

        return ret;
    }

    public static Definition[] FromBinaryFile(string fileName) {
        using var content = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read, 128 * 1024);

        var ret = FromBinary(content);

        return ret;
    }

    public static void ToBinary(Stream content, params Definition[] values) {
        ToBinary(content, values.AsEnumerable());
    }

    public static void ToBinary(Stream data, IEnumerable<Definition> values) {
        using var cs = new GZipStream(data, CompressionLevel.Optimal, true);

        DefinitionJsonSerializer.ToJson(cs, values);
    }

    public static void ToBinaryFile(string fileName, params Definition[] values) {
        ToBinaryFile(fileName, values.AsEnumerable());
    }

    public static void ToBinaryFile(string fileName, IEnumerable<Definition> values) {
        using var content = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read, 128 * 1024);

        ToBinary(content, values);
    }
}
