using System.Collections.Generic;
using System.Collections.Immutable;

namespace MimeDetective.Engine {
    internal abstract class PrefixSegmentFilter {
        public abstract IEnumerable<DefinitionMatcher> Filter(ImmutableArray<byte> Content);
    }


}
