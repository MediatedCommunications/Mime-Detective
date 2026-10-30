using BenchmarkDotNet.Attributes;
using MimeDetective.Benchmark.Support;

namespace MimeDetective.Benchmark;

#pragma warning disable CA1822

public class InspectorBuilderBenchmarks {
    [Benchmark(Baseline = true)]
    public ContentInspector BuildDefault() {
        return ContentInspector.Create(BenchmarkInspectors.Instance.MinimalDefinitions);
    }

    [Benchmark]
    public ContentInspector BuildCondensed() {
        return ContentInspector.Create(BenchmarkInspectors.Instance.CondensedDefinitions);
    }

    [Benchmark]
    public ContentInspector BuildExhaustive() {
        return ContentInspector.Create(BenchmarkInspectors.Instance.ExhaustiveDefinitions);
    }
}
