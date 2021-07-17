using System.Collections.Immutable;

namespace MimeDetective.Engine {
    internal abstract class PrefixSegmentFilterProvider {
        public abstract PrefixSegmentFilter Create(ImmutableArray<DefinitionMatcher> Matchers);
    }


}
