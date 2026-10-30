using System.Collections.Immutable;

namespace MimeDetective.Storage {
    public record DefinitionDeduplicateResults {
        public ImmutableDictionary<string, string> Extensions { get; init; } = ImmutableDictionary<string, string>.Empty;
        public ImmutableDictionary<string, string> MimeTypes { get; init; } = ImmutableDictionary<string, string>.Empty;
        public ImmutableDictionary<string, string> Descriptions { get; init; } = ImmutableDictionary<string, string>.Empty;
        public ImmutableDictionary<OffsetRule, OffsetRule> Prefixes { get; init; } = ImmutableDictionary<OffsetRule, OffsetRule>.Empty;
        public ImmutableDictionary<WildCardRule, WildCardRule> Strings { get; init; } = ImmutableDictionary<WildCardRule, WildCardRule>.Empty;
        public ImmutableDictionary<ImmutableHashSet<Category>, ImmutableHashSet<Category>> Categories { get; init; } = ImmutableDictionary<ImmutableHashSet<Category>, ImmutableHashSet<Category>>.Empty;

        public ImmutableArray<Definition> Definitions { get; init; } = [];

    }
}
