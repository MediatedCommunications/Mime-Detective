using MimeDetective;

var inspector = ContentInspector.Create(MinimalDefinitions.All());

foreach (var file in args) {
    Console.WriteLine($"File: {file}");
    var result = inspector.Inspect(file);

    foreach (var match in result.ByExtension) {

        var FileType = match.Key;

        Console.WriteLine($"  {FileType}");
    }

    Console.WriteLine();
}
