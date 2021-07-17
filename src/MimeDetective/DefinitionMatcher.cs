using MimeDetective.Engine;
using MimeDetective.Storage;
using System.Collections.Immutable;

namespace MimeDetective {
    internal class DefinitionMatcher {

        public DefinitionMatcher(Definition Definition) {
            this.Definition = Definition;
        }

        public Definition Definition { get; }
        public ImmutableArray<PrefixSegmentMatcher> Prefixes { get; init; } = ImmutableArray<PrefixSegmentMatcher>.Empty;
        public ImmutableArray<StringSegmentMatcher> Strings { get; init; } = ImmutableArray<StringSegmentMatcher>.Empty;

    }
}
