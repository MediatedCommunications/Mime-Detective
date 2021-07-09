using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FileDetection.Storage.Trid.v2;

namespace FileDetection.Storage.Trid.v2
{

    /// <summary>
    /// Read and write Trid file definitions
    /// </summary>
    public static class TridSerializer
    {

        public static Definition? FromXml(string Input)
        {
            var Bytes = System.Text.Encoding.UTF8.GetBytes(Input);

            using var MS = new MemoryStream(Bytes);

            return FromXmlStream(MS);
        }

        public static Definition? FromXmlStream(Stream Input) {
            
            var Serializer = new System.Xml.Serialization.XmlSerializer(typeof(Definition));

            var ret = Serializer.Deserialize(Input) as Definition;

            return ret;
        }

        public static Definition? FromXmlFile(string FileName)
        {
            using var fs = System.IO.File.OpenRead(FileName);
            return FromXmlStream(fs);
        }


        public static string ToXml(Trid.v2.Definition Definition)
        {
            using var ms = new MemoryStream();
            ToXmlStream(ms, Definition);

            ms.Position = 0;

            using var sr = new System.IO.StreamReader(ms);
            
            var ret = sr.ReadToEnd();

            return ret;

        }

        public static void ToXmlFile(string FileName, Trid.v2.Definition Definition)
        {
            using var fs = System.IO.File.OpenWrite(FileName);
            ToXmlStream(fs, Definition);
        }

        public static void  ToXmlStream(Stream Output, Trid.v2.Definition Definition)
        {
            var Serializer = new System.Xml.Serialization.XmlSerializer(typeof(Definition));

            Serializer.Serialize(Output, Definition);

        }


    }
}
