using MimeDetective.Definitions;
using MimeDetective.Definitions.Licensing;
using MimeDetective.Storage;
using System.Collections.Immutable;

namespace MimeDetective.Benchmark.Support;

public sealed class BenchmarkInspectors {
    public static BenchmarkInspectors Instance { get; } = new();

    public ImmutableArray<Definition> CondensedDefinitions { get; }
        = new CondensedBuilder { UsageType = UsageType.PersonalNonCommercial }.Build();

    public ImmutableArray<Definition> DefaultDefinitions { get; }
        = MimeDetective.Definitions.Default.All();

    public ImmutableArray<Definition> ExhaustiveDefinitions { get; }
        = new ExhaustiveBuilder { UsageType = UsageType.PersonalNonCommercial }.Build();

    public BenchmarkParameter<ImmutableArray<Definition>>[] Definitions { get; }

    public IContentInspector Default { get; }

    public IContentInspector Condensed { get; }

    public IContentInspector Exhaustive { get; }

    public BenchmarkParameter<IContentInspector>[] Inspectors { get; }

    public BenchmarkInspectors() {
        Definitions = [
            new("Default", DefaultDefinitions),
            new("Condensed", CondensedDefinitions),
            new("Exhaustive", ExhaustiveDefinitions)
        ];

        Condensed = new ContentInspectorBuilder { Definitions = CondensedDefinitions }.Build();
        Exhaustive = new ContentInspectorBuilder { Definitions = ExhaustiveDefinitions }.Build();
        Default = new ContentInspectorBuilder { Definitions = DefaultDefinitions }.Build();

        Inspectors = [
            new("Default", Default),
            new("Condensed", Condensed),
            new("Exhaustive", Exhaustive)
        ];
    }
}
