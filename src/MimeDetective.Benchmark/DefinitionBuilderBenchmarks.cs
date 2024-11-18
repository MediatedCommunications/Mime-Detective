using BenchmarkDotNet.Attributes;
using MimeDetective.Definitions;
using MimeDetective.Definitions.Licensing;
using MimeDetective.Storage;
using System.Collections.Immutable;

namespace MimeDetective.Benchmark
{
    public class DefinitionBuilderBenchmarks
    {
        [Benchmark(Baseline = true)]
        public ImmutableArray<Definition> DefaultDefinitions()
        {
            return Definitions.Default.All();
        }

        [Benchmark]
        public ImmutableArray<Definition> CondensedDefinitions()
        {
            return new CondensedBuilder { UsageType = UsageType.PersonalNonCommercial }.Build();
        }

        [Benchmark]
        public ImmutableArray<Definition> ExhaustiveDefinitions()
        {
            return new ExhaustiveBuilder { UsageType = UsageType.PersonalNonCommercial }.Build();
        }
    }
}
