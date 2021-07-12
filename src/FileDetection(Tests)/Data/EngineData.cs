using FileDetection.Data;
using FileDetection.Storage;

namespace FileDetection.Tests
{
    public static class EngineData
    {
        public static Definition Example()
        {
            var v1 = XmlData.Example();
            var v2 = FileDetection.Storage.Xml.v2.DefinitionExtensions.Modernize(v1);

            return v2; 
        }
    }
}
