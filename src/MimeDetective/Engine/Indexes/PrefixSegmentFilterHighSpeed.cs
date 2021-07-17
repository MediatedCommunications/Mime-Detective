using MimeDetective.Storage;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Text;

namespace MimeDetective.Engine {

    internal class PrefixSegmentFilterHighSpeed : PrefixSegmentFilter {
        public PrefixSegmentFilterHighSpeed(ImmutableArray<DefinitionMatcher> matchers) {
            this.Matchers = matchers;
        }

        public ImmutableArray<DefinitionMatcher> Matchers { get; }

        public override IEnumerable<DefinitionMatcher> Filter(ImmutableArray<byte> Content) {
            return Matchers;
        }
    }


}
