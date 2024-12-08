using Microsoft.AspNetCore.Mvc;
using MimeDetective;
using MimeDetective.Definitions;
using MimeDetective.Engine;
using System.Buffers;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options => {
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

var app = builder.Build();

var inspector = new ContentInspectorBuilder { Definitions = Default.All() }.Build();

// This can be tested with
//   curl --data-binary @example.jpg http://localhost:5016/upload
app.MapPost("/upload", async ([FromHeader(Name = "Content-Length")] int length, Stream stream) => {
    switch (length) {
        case <= 0:
            return Results.BadRequest("Invalid Content-Length");
        case > 10 * 1024 * 1024:
            return Results.BadRequest("File is too large");
    }

    using var buffer = MemoryPool<byte>.Shared.Rent(length);

    await stream.ReadExactlyAsync(buffer.Memory[..length]);

    var result = inspector.Inspect(buffer.Memory[..length].Span);

    return Results.Ok(GetMimeTypes(result));
});

app.Run();

return;

IEnumerable<MimeTypeResult> GetMimeTypes(IReadOnlyCollection<DefinitionMatch> matches) {
    foreach (var match in matches.OrderByDescending(static m => m.Points)) {
        if (match.Type != DefinitionMatchType.Complete) {
            continue;
        }

        var file = match.Definition.File;
        if (string.IsNullOrEmpty(file.MimeType)) {
            continue;
        }

        yield return new(file.MimeType, [.. file.Extensions]);
    }
}

internal sealed record MimeTypeResult(string MimeType, string[] Extensions);

[JsonSerializable(typeof(IEnumerable<MimeTypeResult>))]
internal sealed partial class AppJsonSerializerContext : JsonSerializerContext;
