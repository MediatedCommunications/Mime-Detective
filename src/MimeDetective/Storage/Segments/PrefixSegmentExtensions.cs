using System.Collections.Generic;

namespace MimeDetective.Storage;

public static class PrefixSegmentExtensions {
    /// <summary>
    ///     The exclusive upper bound of the <see cref="PrefixSegment" />'s <see cref="Segment.Content" />
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    public static int ExclusiveEnd(this PrefixSegment This) {
        return This.Start + This.Pattern.Length;
    }

    /// <summary>
    ///     Break a Prefix segment into individual 1-byte segments.
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    public static IEnumerable<PrefixSegment> Singularize(this PrefixSegment This) {
        for (var i = 0; i < This.Pattern.Length; i++) {
            var ret = PrefixSegment.Create(This.Start + i, This.Pattern[i]);

            yield return ret;
        }
    }
}
