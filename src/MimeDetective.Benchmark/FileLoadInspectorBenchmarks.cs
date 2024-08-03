#region

using BenchmarkDotNet.Attributes;
using MimeDetective.Benchmark.Support;
using System.Diagnostics;
using System.Linq;

#endregion

namespace MimeDetective.Benchmark;

public class FileLoadInspectorBenchmarks {
    public BenchmarkParameter<string>[] FilePaths => BenchmarkFiles.Instance.FilePaths;
    public BenchmarkParameter<ContentInspector>[] Inspectors => BenchmarkInspectors.Instance.Inspectors;

    [Benchmark]
    public (string extension, string mimeType) InspectAndInterpret() {
        var results = this.Inspector.Value.Inspect(ContentReader.Default.ReadFromFile(this.TestFile.Value));
        var ext = results.ByFileExtension();
        var mt = results.ByMimeType();

        var extension = ext.OrderByDescending(e => e.Points).FirstOrDefault()?.Extension;
        var mimeType = mt.OrderByDescending(m => m.Points).FirstOrDefault()?.MimeType;

        Debug.WriteLine($"\"{extension}\" and \"{mimeType}\" detected for \"{this.TestFile.Value}\"");

        return (extension, mimeType);
    }

    #region Parameters

    [ParamsSource(nameof(FilePaths))] public BenchmarkParameter<string> TestFile { get; set; }

    [ParamsSource(nameof(Inspectors))] public BenchmarkParameter<ContentInspector> Inspector { get; set; }

    #endregion
}
