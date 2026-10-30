using System.Xml.Serialization;

namespace MimeDetective.Storage.Xml.v2;

[XmlType("Info")]
public class Info {
    [XmlElement("FileType")] public string FileType { get; set; } = string.Empty;

    [XmlElement("Ext")] public string FileExtension { get; set; } = string.Empty;

    [XmlElement("Mime")] public string MimeType { get; set; } = string.Empty;

    [XmlElement("ExtraInfo")] public ExtraInfo ExtraInfo { get; set; } = new();

    [XmlElement("User")] public string Author { get; set; } = string.Empty;

    [XmlElement("E-Mail")] public string AuthorEmail { get; set; } = string.Empty;

    [XmlElement("Home")] public string Website { get; set; } = string.Empty;
}
