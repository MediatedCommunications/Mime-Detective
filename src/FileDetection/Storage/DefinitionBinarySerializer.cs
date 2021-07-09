using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FileDetection.Storage
{
    /// <summary>
    /// Read and write <see cref="Definition"/>s in a compressed binary form.
    /// </summary>
    public static partial class DefinitionBinarySerializer
    {

        private static JsonSerializerOptions BinaryOptions()
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

            var ret = DefinitionJsonSerializer.FromJson(BinaryOptions(), Content);

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
            var Content = DefinitionJsonSerializer.ToJson(BinaryOptions(), Values);

            using var MS = new MemoryStream();
            using var CS = new System.IO.Compression.GZipStream(MS, System.IO.Compression.CompressionLevel.Optimal);
            using var TW = new StreamWriter(CS);
            TW.Write(Content);
            TW.Flush();

            var ret = Array.Empty<byte>();

            if(MS.TryGetBuffer(out var Buffer))
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
