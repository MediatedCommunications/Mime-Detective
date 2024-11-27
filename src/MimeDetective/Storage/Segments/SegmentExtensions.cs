using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;

namespace MimeDetective.Storage;

public static class SegmentExtensions {
    public static Signature ToSignature<T>(this IEnumerable<T> segments)
        where T : Segment {
        List<PrefixSegment>? prefixes = null;
        List<StringSegment>? strings = null;

        foreach (var segment in segments) {
            switch (segment) {
                case PrefixSegment prefix:
                    prefixes ??= [];
                    prefixes.Add(prefix);
                    break;
                case StringSegment str:
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
            => b.Pattern.Length.CompareTo(a.Pattern.Length));

        var ret = new Signature {
            Prefix = prefixes?.ToImmutableArray() ?? [],
            Strings = strings?.ToImmutableArray() ?? []
        };

        return ret;
    }
}
