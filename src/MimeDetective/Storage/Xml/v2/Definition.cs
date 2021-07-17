using System.Collections.Generic;
using System.Xml.Serialization;

namespace MimeDetective.Storage.Xml.v2
{
    [XmlType("T" + "r" + "ID")]
    public class Definition
    {
        [XmlAttribute("ver")]
        public string Version { get; set; } = string.Empty;

        [XmlElement("Info")]
        public Info Info { get; set; } = new();
        
        [XmlElement("General")]
        public General General { get; set; } = new();
        
        [XmlArray("FrontBlock")]
        public List<Pattern> FrontBlock { get; } = new();

        [XmlArray("GlobalStrings")]
        [XmlArrayItem("String")]
        public List<string> GlobalStrings { get; } = new();

    }


}
