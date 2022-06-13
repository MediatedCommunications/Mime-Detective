using System;
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
            };

            return ret;
        }


        public static Definition[] FromBinary(byte[] Data)
        {
            using var MS = new MemoryStream(Data);
            using var CS = new System.IO.Compression.GZipStream(MS, System.IO.Compression.CompressionMode.Decompress);
            using var TR = new StreamReader(CS);
            var Content = TR.ReadToEnd();

            var ret = DefinitionJsonSerializer.FromJson(SerializerOptions(), Content);

            return ret;
        }

        public static Definition[] FromBinaryFile(string FileName)
        {
            var Content = System.IO.File.ReadAllBytes(FileName);

            var ret = FromBinary(Content);

            return ret;
        }

        public static byte[] ToBinary(params Definition[] Values)
        {
            return ToBinary(Values.AsEnumerable());
        }

        public static byte[] ToBinary(IEnumerable<Definition> Values)
        {
            var Content = DefinitionJsonSerializer.ToJson(SerializerOptions(), Values);

            using var ms = new MemoryStream();
            using var CS = new System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionLevel.Optimal);
            using var TW = new StreamWriter(CS);
            TW.Write(Content);
            TW.Flush();

            var ret = Array.Empty<byte>();

            if(ms.TryGetBuffer(out var Buffer))
            {
                ret = Buffer.ToArray();
            }

            return ret;

        }

        

        public static void ToBinaryFile(string FileName, params Definition[] Values) {
            ToBinaryFile(FileName, Values.AsEnumerable());
        }


        public static void ToBinaryFile(string FileName, IEnumerable<Definition> Values)
        {
            var Content = ToBinary(Values);
            System.IO.File.WriteAllBytes(FileName, Content);
        }



    }
}
