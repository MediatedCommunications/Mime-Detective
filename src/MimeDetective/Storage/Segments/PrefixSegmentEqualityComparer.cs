using System.Collections.Generic;

namespace MimeDetective.Storage;

public class PrefixSegmentEqualityComparer : IEqualityComparer<PrefixSegment> {

    public static PrefixSegmentEqualityComparer Instance { get; } = new();

    public bool Equals(PrefixSegment? x, PrefixSegment? y) {
        return true
            && x?.Start == y?.Start
            && EnumerableComparer<byte>.Instance.Equals(x?.Pattern, y?.Pattern)
            ;
    }

    public int GetHashCode(PrefixSegment obj) {
        return obj.GetHashCode();
    }
}