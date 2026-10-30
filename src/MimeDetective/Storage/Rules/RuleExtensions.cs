using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;

namespace MimeDetective.Storage;

public static class RuleExtensions {
    public static Signature ToSignature<T>(this IEnumerable<T> segments)
        where T : Rule {
        List<OffsetRule>? prefixes = null;
        List<WildCardRule>? strings = null;

        foreach (var segment in segments) {
            switch (segment) {
                case OffsetRule prefix:
                    prefixes ??= [];
                    prefixes.Add(prefix);
                    break;
                case WildCardRule str:
                    strings ??= [];
                    strings.Add(str);
                    break;
                default:
                    Debug.Fail("Unknown segment type");
                    break;
            }
        }

        prefixes?.Sort(static (a, b)
            => a.Start.CompareTo(b.Start));
        strings?.Sort(static (a, b)
            => b.Bytes.Length.CompareTo(a.Bytes.Length));

        var ret = new Signature {
            OffsetRules = prefixes?.ToImmutableArray() ?? [],
            WildCardRules = strings?.ToImmutableArray() ?? [],
        };

        return ret;
    }
}
