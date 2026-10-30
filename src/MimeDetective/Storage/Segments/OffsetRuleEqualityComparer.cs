using System.Collections.Generic;

namespace MimeDetective.Storage;

public class OffsetRuleEqualityComparer : IEqualityComparer<OffsetRule> {
    public static OffsetRuleEqualityComparer Instance { get; } = new();

    public bool Equals(OffsetRule? x, OffsetRule? y) {
        return true
            && x?.Start == y?.Start
            && EnumerableComparer<byte>.Instance.Equals(x?.Value, y?.Value)
            ;
    }

    public int GetHashCode(OffsetRule obj) {
        return obj.GetHashCode();
    }
}
