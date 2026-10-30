using MimeDetective;


namespace MimeDetective.Web;

public sealed class InspectorCatalog {
    public const int MaxInspectedBytes = 10 * 1024 * 1024;

    private readonly Dictionary<DefinitionSet, ContentInspector> _inspectors = [];

    public ContentInspector Get(DefinitionSet set) {
        if (_inspectors.TryGetValue(set, out var inspector)) {
            return inspector;
        }

        inspector = Create(set);
        _inspectors[set] = inspector;
        return inspector;
    }

    private static ContentInspector Create(DefinitionSet set) {
        var ret = set switch {
            DefinitionSet.Minimal => MinimalDefinitions.All().ToContentInspector(),
            DefinitionSet.Condensed => CondensedDefinitions.Load(UsageType.PersonalNonCommercial).ToContentInspector(),
            DefinitionSet.Exhaustive => ExhaustiveDefinitions.Load(UsageType.PersonalNonCommercial).ToContentInspector(),
            _ => throw new ArgumentOutOfRangeException(nameof(set), set, "Unknown definition set."),
        };

        return ret;
    }
}
