using System.Collections.Immutable;

namespace MimeDetective.Engine {
    internal class PrefixSegmentFilterProviderHighSpeed : PrefixSegmentFilterProvider {
        public override PrefixSegmentFilter Create(ImmutableArray<DefinitionMatcher> Matchers) {
            return new PrefixSegmentFilterHighSpeed(Matchers);
        }
    }


}
