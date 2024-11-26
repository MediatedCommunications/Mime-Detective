using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace MimeDetective.Tests;

[TestClass]
public class FormatTests {

    [TestMethod]
    public void JsonRoundTrip() {
        var data1 = EngineData.Example();
        using var ms1 = new System.IO.MemoryStream();

        MimeDetective.Storage.DefinitionJsonSerializer.ToJson(ms1, [data1]);
        var json1 = Encoding.UTF8.GetString(ms1.ToArray());

        ms1.Position = 0;
        var data2 = MimeDetective.Storage.DefinitionJsonSerializer.FromJson(ms1);

        using var ms2 = new System.IO.MemoryStream();
        MimeDetective.Storage.DefinitionJsonSerializer.ToJson(ms2, data2);

        var json2 = Encoding.UTF8.GetString(ms1.ToArray());

        CollectionAssert.AreEqual(ms1.ToArray(), ms2.ToArray());
    }

    [TestMethod]
    public void BinaryRoundTrip() {
        var data1 = EngineData.Example();
        using var ms1 = new System.IO.MemoryStream();

        MimeDetective.Storage.DefinitionBinarySerializer.ToBinary(ms1, data1);
        ms1.Position = 0;
        var data2 = MimeDetective.Storage.DefinitionBinarySerializer.FromBinary(ms1);

        using var ms2 = new System.IO.MemoryStream();
        MimeDetective.Storage.DefinitionBinarySerializer.ToBinary(ms2, data2);

        CollectionAssert.AreEqual(ms1.ToArray(), ms2.ToArray());
    }

#if NET5_0_OR_GREATER
    [RequiresUnreferencedCode("This uses XML")]
#endif
    [TestMethod]
    public void TestXmlSchema_FromMemory() {
        var data = XmlData.Example();
        var xml = MimeDetective.Storage.Xml.v2.XmlSerializer.ToXml(data);
        Assert.IsNotNull(xml);
    }

#if NET5_0_OR_GREATER
    [RequiresUnreferencedCode("This uses XML")]
#endif
    [TestMethod]
    public void TestXmlSchema_FromFiles() {
        var folder = Path.Combine(SourceDefinitions.DefinitionRoot(), "0");

        var files = System.IO.Directory.EnumerateFiles(folder, "*.xml");

        foreach (var file in files) {

            var xml = MimeDetective.Storage.Xml.v2.XmlSerializer.FromXmlFile(file);
            Assert.IsNotNull(xml);

        }

    }

    [TestMethod]
    public void BinaryMatchesXml() {
        var data1 = XmlData.Example();
        var data2 = EngineData.Example();

        Assert.AreEqual(data1.General.Date.Year, data2?.Meta?.Created?.At?.Year);
        Assert.AreEqual(data1.General.Date.Month, data2?.Meta?.Created?.At?.Month);
        Assert.AreEqual(data1.General.Date.Day, data2?.Meta?.Created?.At?.Day);
        Assert.AreEqual(data1.General.Time.Hour, data2?.Meta?.Created?.At?.Hour);
        Assert.AreEqual(data1.General.Time.Min, data2?.Meta?.Created?.At?.Minute);
        Assert.AreEqual(data1.General.Time.Sec, data2?.Meta?.Created?.At?.Second);

        Assert.AreEqual(data1.FrontBlock.Count, data2?.Signature.Prefix.Length);


    }





}