using FileDetection.Storage.Xml.v2;

namespace FileDetection.Storage.Xml.v2
{
    public static class DefinitionExtensions
    {
        public static Storage.Definition Modernize(this Definition This)
        {
            return XmlConverter.Convert(This);
        }
    }
}
