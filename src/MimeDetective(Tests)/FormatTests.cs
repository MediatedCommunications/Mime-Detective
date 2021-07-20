using MimeDetective.Definitions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace MimeDetective.Tests {

    [TestClass]
    public class FormatTests
    {
        
        [TestMethod]
        public void JsonRoundTrip()
        {
            var Data1 = EngineData.Example();
            var Json1 = MimeDetective.Storage.DefinitionJsonSerializer.ToJson(Data1);
            var Data2 = MimeDetective.Storage.DefinitionJsonSerializer.FromJson(Json1);
            var Json2 = MimeDetective.Storage.DefinitionJsonSerializer.ToJson(Data2);
            Assert.AreEqual(Json1, Json2);
        }

        [TestMethod]
        public void BinaryRoundTrip()
        {
            var Data1 = EngineData.Example();
            var Json1 = MimeDetective.Storage.DefinitionBinarySerializer.ToBinary(Data1);
            var Data2 = MimeDetective.Storage.DefinitionBinarySerializer.FromBinary(Json1);
            var Json2 = MimeDetective.Storage.DefinitionBinarySerializer.ToBinary(Data2);

            Assert.IsTrue(Enumerable.SequenceEqual(Json1, Json2));
        }

        [TestMethod]
        public void TestXmlSchema_FromMemory()
        {
            var Data = XmlData.Example();
            var xml = MimeDetective.Storage.Xml.v2.XmlSerializer.ToXml(Data);
            Assert.IsNotNull(xml);
        }

        [TestMethod]
        public void TestXmlSchema_FromFiles() {
            var Folder = $@"{SourceDefinitions.DefinitionRoot()}\defs\0\";

            var Files = System.IO.Directory.GetFiles(Folder, "*.xml");

            foreach (var File in Files) {

                var xml = MimeDetective.Storage.Xml.v2.XmlSerializer.FromXmlFile(File);
                Assert.IsNotNull(xml);

            }

        }

        [TestMethod]
        public void BinaryMatchesXml()
        {
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
