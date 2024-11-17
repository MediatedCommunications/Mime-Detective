using BenchmarkDotNet.Attributes;
using MimeDetective.Benchmark.Support;
using MimeDetective.Engine;
using System.Collections.Immutable;

namespace MimeDetective.Benchmark;

public class PreloadedFileInspectorBenchmarks {
    public BenchmarkParameter<byte[]>[] FileContents => BenchmarkFiles.Instance.FileContents;
    public BenchmarkParameter<ContentInspector>[] Inspectors => BenchmarkInspectors.Instance.Inspectors;

    [Benchmark]
    public ImmutableArray<DefinitionMatch> PreloadedInspection() {
        return this.Inspector.Value.Inspect(this.TestFile.Value);
    }

    #region Parameters

    [ParamsSource(nameof(FileContents))] public BenchmarkParameter<byte[]> TestFile { get; set; } = null!;

    [ParamsSource(nameof(Inspectors))] public BenchmarkParameter<ContentInspector> Inspector { get; set; } = null!;

    #endregion
}
