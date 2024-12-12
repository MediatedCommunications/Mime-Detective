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
        public static ImmutableArray<Definition> DefaultDefinitions()
        {
            return Definitions.DefaultDefinitions.All();
        }

        [Benchmark]
        public static ImmutableArray<Definition> CondensedDefinitions()
        {
            return new CondensedBuilder { UsageType = UsageType.PersonalNonCommercial }.Build();
        }

        [Benchmark]
        public static ImmutableArray<Definition> ExhaustiveDefinitions()
        {
            return new ExhaustiveBuilder { UsageType = UsageType.PersonalNonCommercial }.Build();
        }
    }
}
