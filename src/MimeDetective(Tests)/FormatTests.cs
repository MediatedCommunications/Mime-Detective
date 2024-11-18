using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace MimeDetective.Tests {

    [TestClass]
    public class FormatTests {

        [TestMethod]
        public void JsonRoundTrip() {
            var Data1 = EngineData.Example();
            using var Ms1 = new System.IO.MemoryStream();

            MimeDetective.Storage.DefinitionJsonSerializer.ToJson(Ms1, [Data1]);
            var Json1 = Encoding.UTF8.GetString(Ms1.ToArray());

            Ms1.Position = 0;
            var Data2 = MimeDetective.Storage.DefinitionJsonSerializer.FromJson(Ms1);

            using var Ms2 = new System.IO.MemoryStream();
            MimeDetective.Storage.DefinitionJsonSerializer.ToJson(Ms2, Data2);

            var Json2 = Encoding.UTF8.GetString(Ms1.ToArray());

            CollectionAssert.AreEqual(Ms1.ToArray(), Ms2.ToArray());
        }

        [TestMethod]
        public void BinaryRoundTrip() {
            var Data1 = EngineData.Example();
            using var Ms1 = new System.IO.MemoryStream();

            MimeDetective.Storage.DefinitionBinarySerializer.ToBinary(Ms1, Data1);
            Ms1.Position = 0;
            var Data2 = MimeDetective.Storage.DefinitionBinarySerializer.FromBinary(Ms1);

            using var Ms2 = new System.IO.MemoryStream();
            MimeDetective.Storage.DefinitionBinarySerializer.ToBinary(Ms2, Data2);

            CollectionAssert.AreEqual(Ms1.ToArray(), Ms2.ToArray());
        }

#if NET5_0_OR_GREATER
        [RequiresUnreferencedCode("This uses XML")]
#endif
        [TestMethod]
        public void TestXmlSchema_FromMemory() {
            var Data = XmlData.Example();
            var xml = MimeDetective.Storage.Xml.v2.XmlSerializer.ToXml(Data);
            Assert.IsNotNull(xml);
        }

#if NET5_0_OR_GREATER
        [RequiresUnreferencedCode("This uses XML")]
#endif
        [TestMethod]
        public void TestXmlSchema_FromFiles() {
            var Folder = Path.Combine(SourceDefinitions.DefinitionRoot(), "0");

            var Files = System.IO.Directory.EnumerateFiles(Folder, "*.xml");

            foreach (var File in Files) {

                var xml = MimeDetective.Storage.Xml.v2.XmlSerializer.FromXmlFile(File);
                Assert.IsNotNull(xml);

            }

        }

        [TestMethod]
        public void BinaryMatchesXml() {
            var Data1 = XmlData.Example();
            var Data2 = EngineData.Example();

            Assert.AreEqual(Data1.General.Date.Year, Data2?.Meta?.Created?.At?.Year);
            Assert.AreEqual(Data1.General.Date.Month, Data2?.Meta?.Created?.At?.Month);
            Assert.AreEqual(Data1.General.Date.Day, Data2?.Meta?.Created?.At?.Day);
            Assert.AreEqual(Data1.General.Time.Hour, Data2?.Meta?.Created?.At?.Hour);
            Assert.AreEqual(Data1.General.Time.Min, Data2?.Meta?.Created?.At?.Minute);
            Assert.AreEqual(Data1.General.Time.Sec, Data2?.Meta?.Created?.At?.Second);

            Assert.AreEqual(Data1.FrontBlock.Count, Data2?.Signature.Prefix.Length);


        }





    }

}
