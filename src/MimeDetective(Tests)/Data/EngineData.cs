using MimeDetective.Storage;

namespace MimeDetective.Tests;

public static class EngineData {
    public static Definition Example() {
        var v1 = XmlData.Example();
        var v2 = MimeDetective.Storage.Xml.v2.DefinitionExtensions.Modernize(v1);

        return v2;
    }
}