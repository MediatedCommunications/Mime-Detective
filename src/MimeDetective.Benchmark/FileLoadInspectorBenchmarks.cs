#region

using BenchmarkDotNet.Attributes;
using MimeDetective.Benchmark.Support;
using System.Diagnostics;
using System.Linq;

#endregion

#pragma warning disable CA1822

namespace MimeDetective.Benchmark;

public class FileLoadInspectorBenchmarks {
    public BenchmarkParameter<string>[] FilePaths => BenchmarkFiles.Instance.FilePaths;
    public BenchmarkParameter<ContentInspector>[] Inspectors => BenchmarkInspectors.Instance.Inspectors;

    [Benchmark]
    public (string? extension, string? mimeType) InspectAndInterpret() {
        var results = Inspector.Value.Inspect(ContentReader.Default.ReadFromFile(TestFile.Value));

        var extension = results.ByExtension.FirstOrDefault()?.Key;
        var mimeType = results.ByMimeType.FirstOrDefault()?.Key;

        Debug.WriteLine($"\"{extension}\" and \"{mimeType}\" detected for \"{TestFile.Value}\"");

        return (extension, mimeType);
    }

    #region Parameters

    [ParamsSource(nameof(FilePaths))] public BenchmarkParameter<string> TestFile { get; set; } = null!;

    [ParamsSource(nameof(Inspectors))] public BenchmarkParameter<ContentInspector> Inspector { get; set; } = null!;

    #endregion
}
