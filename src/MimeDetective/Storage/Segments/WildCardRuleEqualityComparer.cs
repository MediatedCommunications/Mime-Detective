using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MimeDetective.Storage;

public class WildCardRuleEqualityComparer : IEqualityComparer<WildCardRule> {
    public static WildCardRuleEqualityComparer Instance { get; } = new();

    public bool Equals(WildCardRule? x, WildCardRule? y) {
        return EnumerableComparer<byte>.Instance.Equals(x?.Value, y?.Value);
    }

    public int GetHashCode(WildCardRule obj) {
        return obj.GetHashCode();
    }
}

