using System.Collections.Generic;

namespace MimeDetective.Storage;

public class StringSegmentEqualityComparer : IEqualityComparer<StringSegment> {
    public static StringSegmentEqualityComparer Instance { get; } = new();

    public bool Equals(StringSegment? x, StringSegment? y) {
        return EnumerableComparer<byte>.Instance.Equals(x?.Pattern, y?.Pattern);
    }

    public int GetHashCode(StringSegment obj) {
        return obj.GetHashCode();
    }
}
