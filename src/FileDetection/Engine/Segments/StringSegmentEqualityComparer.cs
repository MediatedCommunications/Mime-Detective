using FileDetection.Storage;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FileDetection.Engine {
    public class StringSegmentEqualityComparer : IEqualityComparer<StringSegment> {

        public static StringSegmentEqualityComparer Instance { get; } = new();

        public bool Equals(StringSegment? x, StringSegment? y) {
            return ArrayComparer<byte>.Instance.Equals(x?.Pattern, y?.Pattern);
        }

        public int GetHashCode(StringSegment obj) {
            return obj.GetHashCode();
        }
    }

}
