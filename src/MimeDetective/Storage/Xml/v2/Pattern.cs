using System.Xml.Serialization;

namespace MimeDetective.Storage.Xml.v2
{

    [XmlType("Pattern")]
    public class Pattern
    {
        [XmlElement("Bytes")]
        public string? Bytes { get; set; }
        
        [XmlElement("ASCII")]
        public string? ASCII { get; set; }
        
        [XmlElement("Pos")]
        public int Position { get; set; }
    }
}
