using FileDetection.Data;
using FileDetection.Data.Engine;

namespace FileDetection.Tests
{
    public static class EngineData
    {
        public static Definition Example()
        {
            var v1 = XmlData.Example();
            var v2 = FileDetection.Data.Trid.v2.DefinitionExtensions.Modernize(v1);

            return v2; 
        }
    }
}
