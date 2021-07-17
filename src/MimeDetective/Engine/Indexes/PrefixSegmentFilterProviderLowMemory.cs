using System.Collections.Immutable;

namespace MimeDetective.Engine {
    internal class PrefixSegmentFilterProviderLowMemory : PrefixSegmentFilterProvider {
        public override PrefixSegmentFilter Create(ImmutableArray<DefinitionMatcher> Matchers) {
            return new PrefixSegmentFilterLowMemory(Matchers);
        }
    }


}
