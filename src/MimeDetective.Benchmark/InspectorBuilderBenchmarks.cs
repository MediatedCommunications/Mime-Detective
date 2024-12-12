using BenchmarkDotNet.Attributes;
using MimeDetective.Benchmark.Support;

namespace MimeDetective.Benchmark;

#pragma warning disable CA1822

public class InspectorBuilderBenchmarks {
    [Benchmark(Baseline = true)]
    public IContentInspector BuildDefault() {
        return new ContentInspectorBuilder { Definitions = BenchmarkInspectors.Instance.DefaultDefinitions }.Build();
    }

    [Benchmark]
    public IContentInspector BuildCondensed() {
        return new ContentInspectorBuilder { Definitions = BenchmarkInspectors.Instance.CondensedDefinitions }.Build();
    }

    [Benchmark]
    public IContentInspector BuildExhaustive() {
        return new ContentInspectorBuilder { Definitions = BenchmarkInspectors.Instance.ExhaustiveDefinitions }.Build();
    }
}
