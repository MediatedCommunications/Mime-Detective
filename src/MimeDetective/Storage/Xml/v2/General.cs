using System.Xml.Serialization;

namespace MimeDetective.Storage.Xml.v2
{

    [XmlType("General")]
    public class General
    {
        [XmlElement("FileNum")]
        public long FileNum { get; set; }

        [XmlElement("Refine")]
        public string Refine { get; set; } = string.Empty;

        [XmlElement("CheckStrings")]
        public string CheckStrings { get; set; } = string.Empty;
        
        public Date Date { get; } = new();
        
        public Time Time { get; } = new();
        
        [XmlElement("Creator")]
        public string Creator { get; set; } = string.Empty;
    }
}
