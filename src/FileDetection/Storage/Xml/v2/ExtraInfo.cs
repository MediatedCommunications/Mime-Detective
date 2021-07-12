using System.Xml.Serialization;

namespace FileDetection.Storage.Xml.v2
{

    [XmlType("ExtraInfo")]
    public class ExtraInfo
    {
        [XmlElement("Rem")]
        public string Remark { get; set; } = string.Empty;
        
        [XmlElement("RefURL")]
        public string ReferenceUrl { get; set; } = string.Empty;
    }
}
