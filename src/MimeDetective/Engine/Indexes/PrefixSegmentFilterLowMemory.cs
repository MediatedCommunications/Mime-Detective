using System.Collections.Generic;
using System.Collections.Immutable;

namespace MimeDetective.Engine {
    internal class PrefixSegmentFilterLowMemory : PrefixSegmentFilter {
        public PrefixSegmentFilterLowMemory(ImmutableArray<DefinitionMatcher> matchers) {
            this.Matchers = matchers;
        }

        public ImmutableArray<DefinitionMatcher> Matchers { get; }

        public override IEnumerable<DefinitionMatcher> Filter(ImmutableArray<byte> Content) {
            return Matchers;
        }
    }


}
