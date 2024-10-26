using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace MimeDetective.Storage {
    /// <summary>
    /// Read and write <see cref="Definition"/>s in a compressed binary form.
    /// </summary>
    public static partial class DefinitionBinarySerializer
    {

        private static JsonSerializerOptions SerializerOptions()
        {
            var ret = new JsonSerializerOptions()
            {
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = true,
                WriteIndented = false,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault,
#if NET7_0_OR_GREATER
                TypeInfoResolver = MimeDetectiveSourceGeneratedSerializer.Default
#endif
            };

            return ret;
        }


        public static Definition[] FromBinary(Stream Data)
        {
            using var CS = new System.IO.Compression.GZipStream(Data, System.IO.Compression.CompressionMode.Decompress, true);
            using var BS = new BufferedStream(CS, 128 * 1024);

            var ret = DefinitionJsonSerializer.FromJson(BS);

            return ret;
        }

        public static Definition[] FromBinaryFile(string FileName)
        {
            using var Content = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read, 128 * 1024);

            var ret = FromBinary(Content);

            return ret;
        }

        public static void ToBinary(Stream Content, params Definition[] Values)
        {
            ToBinary(Content, Values.AsEnumerable());
        }

        public static void ToBinary(Stream Data, IEnumerable<Definition> Values)
        {
            using var CS = new System.IO.Compression.GZipStream(Data, System.IO.Compression.CompressionLevel.Optimal, true);

            DefinitionJsonSerializer.ToJson(CS, Values);
        }

        public static void ToBinaryFile(string FileName, params Definition[] Values) {
            ToBinaryFile(FileName, Values.AsEnumerable());
        }

        public static void ToBinaryFile(string FileName, IEnumerable<Definition> Values)
        {
            using var Content = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.Read, 128 * 1024);

            ToBinary(Content, Values);
        }

    }
}
