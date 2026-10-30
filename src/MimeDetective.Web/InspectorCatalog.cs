using MimeDetective;
using MimeDetective.Definitions;
using MimeDetective.Definitions.Licensing;

namespace MimeDetective.Web;

public sealed class InspectorCatalog {
    public const int MaxInspectedBytes = 10 * 1024 * 1024;

    private readonly Dictionary<DefinitionSet, IContentInspector> _inspectors = [];

    public IContentInspector Get(DefinitionSet set) {
        if (_inspectors.TryGetValue(set, out var inspector)) {
            return inspector;
        }

        inspector = Create(set);
        _inspectors[set] = inspector;
        return inspector;
    }

    private static IContentInspector Create(DefinitionSet set) {
        var definitions = set switch {
            DefinitionSet.Minimal => MinimalDefinitions.All(),
            DefinitionSet.Condensed => new CondensedBuilder {
                UsageType = UsageType.PersonalNonCommercial
            }.Build(),
            DefinitionSet.Exhaustive => new ExhaustiveBuilder {
                UsageType = UsageType.PersonalNonCommercial
            }.Build(),
            _ => throw new ArgumentOutOfRangeException(nameof(set), set, "Unknown definition set.")
        };

        return new ContentInspectorBuilder {
            Definitions = definitions,
            Parallel = false
        }.Build();
    }
}
