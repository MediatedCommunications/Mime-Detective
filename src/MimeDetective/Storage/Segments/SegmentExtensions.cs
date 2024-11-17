using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;

namespace MimeDetective.Storage;

public static class SegmentExtensions {
    public static Signature ToSignature<T>(this IEnumerable<T> Segments)
        where T : Segment {
        List<PrefixSegment>? Prefixes = null;
        List<StringSegment>? Strings = null;

        foreach (var segment in Segments) {
            switch (segment) {
                case PrefixSegment prefix:
                    Prefixes ??= [];
                    Prefixes.Add(prefix);
                    break;
                case StringSegment str:
                    Strings ??= [];
                    Strings.Add(str);
                    break;
                default:
                    Debug.Fail("Unknown segment type");
                    break;
            }
        }

        Prefixes?.Sort(static (a, b)
            => a.Start.CompareTo(b.Start));
        Strings?.Sort(static (a, b)
            => b.Pattern.Length.CompareTo(a.Pattern.Length));

        var ret = new Signature {
            Prefix = Prefixes?.ToImmutableArray() ?? [],
            Strings = Strings?.ToImmutableArray() ?? []
        };

        return ret;
    }
}
