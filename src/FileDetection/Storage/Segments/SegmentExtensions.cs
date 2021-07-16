using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace FileDetection.Storage {
    public static class SegmentExtensions {
        public static Signature ToSignature<T>(this IEnumerable<T> Segments) where T: Segment {
            var Prefixes = Segments
                .OfType<PrefixSegment>()
                .OrderBy(x => x.Start)
                .ToImmutableArray()
                ;

            var Strings = Segments
                .OfType<StringSegment>()
                .OrderByDescending(x => x.Pattern.Length)
                .ToImmutableArray()
                ;

            var ret = new Signature() {
                Prefix = Prefixes,
                Strings = Strings,
            };

            return ret;
        }
    }

}
