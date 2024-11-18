using BenchmarkDotNet.Attributes;
using MimeDetective.Benchmark.Support;
using MimeDetective.Engine;
using MimeDetective.MemoryMapping;
using System.Collections.Immutable;

namespace MimeDetective.Benchmark;

public class LoadFileBenchmarks {
    public BenchmarkParameter<string>[] FilePaths => BenchmarkFiles.Instance.FilePaths;

    #region Parameters

    [ParamsSource(nameof(FilePaths))] public BenchmarkParameter<string> TestFile { get; set; } = null!;

    #endregion

    [Benchmark(Baseline = true)]
    public ImmutableArray<DefinitionMatch> ReadFromFile() {
        return BenchmarkInspectors.Instance.Default.Inspect(ContentReader.Default.ReadFromFile(this.TestFile.Value));
    }

    [Benchmark]
    public ImmutableArray<DefinitionMatch> InspectFilePath() {
        return BenchmarkInspectors.Instance.Default.Inspect(this.TestFile.Value);
    }

    [Benchmark]
    public ImmutableArray<DefinitionMatch> MapFile() {
        return BenchmarkInspectors.Instance.Default.InspectMemoryMapped(this.TestFile.Value);
    }
}
