using BenchmarkDotNet.Attributes;
using MimeDetective.Benchmark.Support;

namespace MimeDetective.Benchmark;

public class InspectorBuilderBenchmarks {
    [Benchmark(Baseline = true)]
    public ContentInspector BuildDefault() {
        return new ContentInspectorBuilder { Definitions = BenchmarkInspectors.Instance.DefaultDefinitions }.Build();
    }

    [Benchmark]
    public ContentInspector BuildCondensed() {
        return new ContentInspectorBuilder { Definitions = BenchmarkInspectors.Instance.CondensedDefinitions }.Build();
    }

    [Benchmark]
    public ContentInspector BuildExhaustive() {
        return new ContentInspectorBuilder { Definitions = BenchmarkInspectors.Instance.ExhaustiveDefinitions }.Build();
    }
}
