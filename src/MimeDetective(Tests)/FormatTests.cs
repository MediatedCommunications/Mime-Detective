using Microsoft.VisualStudio.TestTools.UnitTesting;
using MimeDetective.Storage;
using MimeDetective.Storage.Xml.v2;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace MimeDetective.Tests;

[TestClass]
public class FormatTests {
    [TestMethod]
    public void JsonRoundTrip() {
        var data1 = EngineData.Example();
        using var ms1 = new MemoryStream();

        DefinitionJsonSerializer.ToJson(ms1, [data1]);
        var json1 = Encoding.UTF8.GetString(ms1.ToArray());

        ms1.Position = 0;
        var data2 = DefinitionJsonSerializer.FromJson(ms1);

        using var ms2 = new MemoryStream();
        DefinitionJsonSerializer.ToJson(ms2, data2);

        var json2 = Encoding.UTF8.GetString(ms1.ToArray());

        CollectionAssert.AreEqual(ms1.ToArray(), ms2.ToArray());
    }

    [TestMethod]
    public void BinaryRoundTrip() {
        var data1 = EngineData.Example();
        using var ms1 = new MemoryStream();

        DefinitionBinarySerializer.ToBinary(ms1, data1);
        ms1.Position = 0;
        var data2 = DefinitionBinarySerializer.FromBinary(ms1);

        using var ms2 = new MemoryStream();
        DefinitionBinarySerializer.ToBinary(ms2, data2);

        CollectionAssert.AreEqual(ms1.ToArray(), ms2.ToArray());
    }

#if NET5_0_OR_GREATER
    [RequiresUnreferencedCode("This uses XML")]
#endif
    [TestMethod]
    public void TestXmlSchema_FromMemory() {
        var data = XmlData.Example();
        var xml = XmlSerializer.ToXml(data);
        Assert.IsNotNull(xml);
    }

#if NET5_0_OR_GREATER
    [RequiresUnreferencedCode("This uses XML")]
#endif
    [TestMethod]
    public void TestXmlSchema_FromFiles() {
        var folder = Path.Combine(SourceDefinitions.DefinitionRoot(), "0");

        var files = Directory.EnumerateFiles(folder, "*.xml");

        foreach (var file in files) {
            var xml = XmlSerializer.FromXmlFile(file);
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
