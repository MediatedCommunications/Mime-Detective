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
        = MimeDetective.Definitions.DefaultDefinitions.All();

    public ImmutableArray<Definition> ExhaustiveDefinitions { get; }
        = new ExhaustiveBuilder { UsageType = UsageType.PersonalNonCommercial }.Build();

    public BenchmarkParameter<ImmutableArray<Definition>>[] Definitions { get; }

    public IContentInspector Default { get; }

    public IContentInspector Condensed { get; }

    public IContentInspector Exhaustive { get; }

    public BenchmarkParameter<IContentInspector>[] Inspectors { get; }

    public BenchmarkInspectors() {
        this.Definitions = [
            new("Default", this.DefaultDefinitions),
            new("Condensed", this.CondensedDefinitions),
            new("Exhaustive", this.ExhaustiveDefinitions)
        ];

        this.Condensed = new ContentInspectorBuilder { Definitions = this.CondensedDefinitions }.Build();
        this.Exhaustive = new ContentInspectorBuilder { Definitions = this.ExhaustiveDefinitions }.Build();
        this.Default = new ContentInspectorBuilder { Definitions = this.DefaultDefinitions }.Build();

        this.Inspectors = [
            new("Default", this.Default),
            new("Condensed", this.Condensed),
            new("Exhaustive", this.Exhaustive)
        ];
    }
}
