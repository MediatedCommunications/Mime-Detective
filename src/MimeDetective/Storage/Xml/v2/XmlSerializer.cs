using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Xml;

namespace MimeDetective.Storage.Xml.v2;

/// <summary>
///     Read and write xml file definitions
/// </summary>
public static class XmlSerializer {
#if NET8_0_OR_GREATER
    [RequiresUnreferencedCode("XmlSerializer deserializes Definition")]
#endif
    public static Definition? FromXml(string input) {
        var bytes = Encoding.UTF8.GetBytes(input);

        using var ms = new MemoryStream(bytes);

        return FromXmlStream(ms);
    }

#if NET8_0_OR_GREATER
    [RequiresUnreferencedCode("XmlSerializer deserializes Definition")]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(Definition))]
#endif
    public static Definition? FromXmlStream(Stream input) {
        var serializer = new System.Xml.Serialization.XmlSerializer(typeof(Definition));

        var ret = serializer.Deserialize(XmlReader.Create(input)) as Definition;

        return ret;
    }

#if NET8_0_OR_GREATER
    [RequiresUnreferencedCode("XmlSerializer deserializes Definition")]
#endif
    public static Definition? FromXmlFile(string fileName) {
        using var fs = File.OpenRead(fileName);
        return FromXmlStream(fs);
    }


#if NET8_0_OR_GREATER
    [RequiresUnreferencedCode("XmlSerializer serializes Definition")]
#endif
    public static string ToXml(Definition definition) {
        using var ms = new MemoryStream();
        ToXmlStream(ms, definition);

        ms.Position = 0;

        using var sr = new StreamReader(ms);

        var ret = sr.ReadToEnd();

        return ret;
    }

#if NET8_0_OR_GREATER
    [RequiresUnreferencedCode("XmlSerializer serializes Definition")]
#endif
    public static void ToXmlFile(string fileName, Definition definition) {
        using var fs = File.OpenWrite(fileName);
        ToXmlStream(fs, definition);
    }

#if NET8_0_OR_GREATER
    [RequiresUnreferencedCode("XmlSerializer serializes Definition")]
#endif
    public static void ToXmlStream(Stream output, Definition definition) {
        var serializer = new System.Xml.Serialization.XmlSerializer(typeof(Definition));

        serializer.Serialize(output, definition);
    }
}
