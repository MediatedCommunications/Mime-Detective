using System.Collections.Generic;
using System.Linq;

namespace MimeDetective.Storage;

public static class PrefixSegmentExtensions {
    /// <summary>
    ///     The exclusive upper bound of the <see cref="PrefixSegment" />'s <see cref="Segment.Content" />
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    public static int ExclusiveEnd(this PrefixSegment @this) {
        return @this.Start + @this.Pattern.Length;
    }

    public static byte?[] TryGetRange(this IEnumerable<PrefixSegment> @this, int index, int length) {
        var ret = new byte?[length];
        var segments = @this.OrderBy(x => x.Start).ToArray();
        var segmentIndex = 0;

        for (var i = 0; i < length; i++) {
            var byteIndex = index + i;

            while (segmentIndex < segments.Length &&
                (byteIndex < segments[segmentIndex].Start || byteIndex >= segments[segmentIndex].ExclusiveEnd())) {
                segmentIndex += 1;
            }

            if (segmentIndex < segments.Length) {
                ret[i] = segments[segmentIndex].TryGetPatternAt(byteIndex);
            }
        }


        return ret;
    }

    public static byte? TryGetPatternAt(this PrefixSegment? @this, int index) {
        var ret = default(byte?);

        if (TryGetPatternAt(@this, index, out var v1)) {
            ret = v1;
        }

        return ret;
    }

    public static bool TryGetPatternAt(this PrefixSegment? @this, int index, out byte value) {
        var ret = false;

        value = default;

        if (@this is not null && index >= @this.Start && index < @this.ExclusiveEnd()) {
            value = @this.GetPatternAt(index);
            ret = true;
        }

        return ret;
    }

    public static byte GetPatternAt(this PrefixSegment @this, int index) {
        var ret = @this.Pattern[index - @this.Start];

        return ret;
    }

    /// <summary>
    ///     Break a Prefix segment into individual 1-byte segments.
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    public static IEnumerable<PrefixSegment> Singularize(this PrefixSegment @this) {
        for (var i = 0; i < @this.Pattern.Length; i++) {
            var ret = PrefixSegment.Create(@this.Start + i, @this.Pattern[i]);

            yield return ret;
        }
    }
}
