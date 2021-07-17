using MimeDetective.Storage;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MimeDetective.Engine {
    public class PrefixSegmentEqualityComparer : IEqualityComparer<PrefixSegment> {

        public static PrefixSegmentEqualityComparer Instance { get; } = new();

        public bool Equals(PrefixSegment? x, PrefixSegment? y) {
            return true
                && x?.Start == y?.Start
                && ArrayComparer<byte>.Instance.Equals(x?.Pattern, y?.Pattern)
                ;
        }

        public int GetHashCode(PrefixSegment obj) {
            return obj.GetHashCode();
        }
    }

}
