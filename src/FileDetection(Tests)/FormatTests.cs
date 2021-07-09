using FileDetection.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace FileDetection.Tests
{


    [TestClass]
    public class FormatTests
    {
        
        [TestMethod]
        public void JsonRoundTrip()
        {
            var Data1 = EngineData.Example();
            var Json1 = FileDetection.Data.Engine.DefinitionJsonSerializer.ToJson(Data1);
            var Data2 = FileDetection.Data.Engine.DefinitionJsonSerializer.FromJson(Json1);
            var Json2 = FileDetection.Data.Engine.DefinitionJsonSerializer.ToJson(Data2);

            Assert.AreEqual(Json1, Json2);
        }

        [TestMethod]
        public void BinaryRoundTrip()
        {
            var Data1 = EngineData.Example();
            var Json1 = FileDetection.Data.Engine.DefinitionBinarySerializer.ToBinary(Data1);
            var Data2 = FileDetection.Data.Engine.DefinitionBinarySerializer.FromBinary(Json1);
            var Json2 = FileDetection.Data.Engine.DefinitionBinarySerializer.ToBinary(Data2);

            Assert.IsTrue(Enumerable.SequenceEqual(Json1, Json2));
        }

        [TestMethod]
        public void TestXmlSchema()
        {
            var Data = XmlData.Example();
            var xml1 = FileDetection.Data.Trid.v2.TridSerializer.ToXml(Data);

            var xml2 = FileDetection.Data.Trid.v2.TridSerializer.FromXmlFile($@"C:\Users\Faster Law\Downloads\triddefs_xml\defs\0\{{sa}}proj.trid.xml");

            Assert.IsNotNull(xml1);
            Assert.IsNotNull(xml2);

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
