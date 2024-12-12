using MimeDetective;
using MimeDetective.Definitions;
using MimeDetective.Engine;
using MimeDetective.MemoryMapping;

var inspector = new ContentInspectorBuilder { Definitions = Default.All() }.Build();

foreach (var file in args) {
    Console.WriteLine($"File: {file}");
    var result = inspector.InspectMemoryMapped(file);

    foreach (var match in result.OrderByDescending(static m => m.Points)) {
        if (match.Type != DefinitionMatchType.Complete) {
            continue;
        }

        var fileType = match.Definition.File;
        if (string.IsNullOrEmpty(fileType.MimeType)) {
            continue;
        }

        Console.WriteLine($"  {fileType.MimeType} ({string.Join(", ", fileType.Extensions)})");
    }

    Console.WriteLine();
}
