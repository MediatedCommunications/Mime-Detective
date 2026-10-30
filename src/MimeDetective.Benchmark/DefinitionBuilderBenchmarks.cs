using BenchmarkDotNet.Attributes;
using MimeDetective.Definitions;

using MimeDetective.Storage;
using System.Collections.Immutable;

namespace MimeDetective.Benchmark
{
    public class DefinitionBuilderBenchmarks
    {
        [Benchmark(Baseline = true)]
        public ImmutableArray<Definition> MinimalDefinitions() {
            return [.. MimeDetective.MinimalDefinitions.All()];
        }

        [Benchmark]
        public ImmutableArray<Definition> CondensedDefinitions()
        {
            return [.. MimeDetective.CondensedDefinitions.Load(UsageType.PersonalNonCommercial)];
        }

        [Benchmark]
        public ImmutableArray<Definition> ExhaustiveDefinitions()
        {
            return [.. MimeDetective.ExhaustiveDefinitions.Load(UsageType.PersonalNonCommercial)];
        }
    }
}
