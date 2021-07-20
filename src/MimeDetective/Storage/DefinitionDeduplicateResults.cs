using System.Collections.Immutable;

namespace MimeDetective.Storage {
    public record DefinitionDeduplicateResults {
        public ImmutableDictionary<string, string> Extensions { get; init; } = ImmutableDictionary<string, string>.Empty;
        public ImmutableDictionary<string, string> MimeTypes { get; init; } = ImmutableDictionary<string, string>.Empty;
        public ImmutableDictionary<string, string> Descriptions { get; init; } = ImmutableDictionary<string, string>.Empty;
        public ImmutableDictionary<PrefixSegment, PrefixSegment> Prefixes { get; init; } = ImmutableDictionary<PrefixSegment, PrefixSegment>.Empty;
        public ImmutableDictionary<StringSegment, StringSegment> Strings { get; init; } = ImmutableDictionary<StringSegment, StringSegment>.Empty;
        public ImmutableDictionary<ImmutableHashSet<Category>, ImmutableHashSet<Category>> Categories { get; init; } = ImmutableDictionary<ImmutableHashSet<Category>, ImmutableHashSet<Category>>.Empty;

        public ImmutableArray<Definition> Definitions { get; init; } = ImmutableArray<Definition>.Empty;

    }
}
