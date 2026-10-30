using BenchmarkDotNet.Attributes;
using MimeDetective.Benchmark.Support;

using System.Collections.Immutable;

namespace MimeDetective.Benchmark;

#pragma warning disable CA1822

public class LoadFileBenchmarks {
    public BenchmarkParameter<string>[] FilePaths => BenchmarkFiles.Instance.FilePaths;

    #region Parameters

    [ParamsSource(nameof(FilePaths))] public BenchmarkParameter<string> TestFile { get; set; } = null!;

    #endregion

    [Benchmark(Baseline = true)]
    public ContentInspectorResults ReadFromFile() {
        return BenchmarkInspectors.Instance.Minimal.Inspect(ContentReader.Default.ReadFromFile(TestFile.Value));
    }

    [Benchmark]
    public ContentInspectorResults InspectFilePath() {
        return BenchmarkInspectors.Instance.Minimal.Inspect(TestFile.Value, ContentReader.Default);
    }

    [Benchmark]
    public ContentInspectorResults MapFile() {
        return BenchmarkInspectors.Instance.Minimal.Inspect(TestFile.Value);
    }
}
