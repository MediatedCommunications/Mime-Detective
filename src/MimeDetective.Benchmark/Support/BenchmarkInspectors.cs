using MimeDetective.Definitions;
using MimeDetective.Storage;
using System.Collections.Immutable;

namespace MimeDetective.Benchmark.Support;

public sealed class BenchmarkInspectors {
    public static BenchmarkInspectors Instance { get; } = new();

    public ImmutableArray<Definition> CondensedDefinitions { get; }
        = [.. MimeDetective.CondensedDefinitions.Load(UsageType.PersonalNonCommercial)];

    public ImmutableArray<Definition> MinimalDefinitions { get; }
        = MimeDetective.MinimalDefinitions.All();

    public ImmutableArray<Definition> ExhaustiveDefinitions { get; }
        = [.. MimeDetective.ExhaustiveDefinitions.Load(UsageType.PersonalNonCommercial)];

    public BenchmarkParameter<ImmutableArray<Definition>>[] Definitions { get; }

    public ContentInspector Minimal { get; }

    public ContentInspector Condensed { get; }

    public ContentInspector Exhaustive { get; }

    public BenchmarkParameter<ContentInspector>[] Inspectors { get; }

    public BenchmarkInspectors() {
        this.Definitions = [
            new("Minimal", this.MinimalDefinitions),
            new("Condensed", this.CondensedDefinitions),
            new("Exhaustive", this.ExhaustiveDefinitions),
        ];

        this.Condensed = ContentInspector.Create(CondensedDefinitions);
        this.Exhaustive = ContentInspector.Create(ExhaustiveDefinitions);
        this.Minimal = ContentInspector.Create(MinimalDefinitions);

        this.Inspectors = [
            new("Minimal", this.Minimal),
            new("Condensed", this.Condensed),
            new("Exhaustive", this.Exhaustive),
        ];
    }
}
