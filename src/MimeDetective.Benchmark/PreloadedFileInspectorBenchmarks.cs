using BenchmarkDotNet.Attributes;
using MimeDetective.Benchmark.Support;
using MimeDetective.Engine;
using System.Collections.Immutable;

namespace MimeDetective.Benchmark;

#pragma warning disable CA1822

public class PreloadedFileInspectorBenchmarks {
    public BenchmarkParameter<byte[]>[] FileContents => BenchmarkFiles.Instance.FileContents;
    public BenchmarkParameter<IContentInspector>[] Inspectors => BenchmarkInspectors.Instance.Inspectors;

    [Benchmark]
    public ImmutableArray<DefinitionMatch> PreloadedInspection() {
        return Inspector.Value.Inspect(TestFile.Value);
    }

    #region Parameters

    [ParamsSource(nameof(FileContents))] public BenchmarkParameter<byte[]> TestFile { get; set; } = null!;

    [ParamsSource(nameof(Inspectors))] public BenchmarkParameter<IContentInspector> Inspector { get; set; } = null!;

    #endregion
}
