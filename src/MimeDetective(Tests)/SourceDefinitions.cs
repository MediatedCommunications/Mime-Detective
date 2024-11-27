using System;

namespace MimeDetective.Tests;

public static class SourceDefinitions {

    public static string DefinitionRoot() {
        var profile = System.Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        var folder = $@"{profile}\Downloads\defs_xml";

        return folder;
    }

}