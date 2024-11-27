using MimeDetective.Storage;
using System;

namespace MimeDetective.Engine;

/// <summary>
/// An <see cref="ISegmentMatcher"/> that matches a <see cref="StringSegment"/> against content.
/// </summary>
internal class StringSegmentMatcherBrute : StringSegmentMatcher {
    public StringSegmentMatcherBrute(StringSegment segment) : base(segment) {

    }

    public override SegmentMatch Match(ReadOnlySpan<byte> haystack) {
        SegmentMatch ret = NoSegmentMatch.Instance;

        for (var i = 0; i <= haystack.Length - Segment.Pattern.Length; i++) {
            if (Match(haystack, Segment.Pattern.AsSpan(), i)) {
                ret = new StringSegmentMatch() {
                    Segment = Segment,
                    Index = i
                };
                break;
            }
        }

        return ret;
    }

    private static bool Match(ReadOnlySpan<byte> haystack, ReadOnlySpan<byte> needle, int start) {
        var ret = true;

        for (var i = 0; i < needle.Length; i++) {
            if (needle[i] != haystack[i + start]) {
                ret = false;
                break;
            }
        }

        return ret;

    }

}