using BenchmarkDotNet.Attributes;
using MimeDetective.Benchmark.Support;
using System.Collections.Immutable;

namespace MimeDetective.Benchmark;

#pragma warning disable CA1822

public class PreloadedFileInspectorBenchmarks {
    public BenchmarkParameter<byte[]>[] FileContents => BenchmarkFiles.Instance.FileContents;
    public BenchmarkParameter<ContentInspector>[] Inspectors => BenchmarkInspectors.Instance.Inspectors;

    [Benchmark]
    public ContentInspectorResults PreloadedInspection() {
        return Inspector.Value.Inspect(TestFile.Value);
    }

    #region Parameters

    [ParamsSource(nameof(FileContents))] public BenchmarkParameter<byte[]> TestFile { get; set; } = null!;

    [ParamsSource(nameof(Inspectors))] public BenchmarkParameter<ContentInspector> Inspector { get; set; } = null!;

    #endregion
}
