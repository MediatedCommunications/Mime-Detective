using MimeDetective.Storage;
using System;

namespace MimeDetective.Engine;

/// <summary>
///     An <see cref="ISegmentMatcher" /> that matches a <see cref="StringSegment" /> against content.
/// </summary>
internal class StringSegmentMatcherBrute : StringSegmentMatcher {
    public StringSegmentMatcherBrute(StringSegment segment) : base(segment) { }

    public override SegmentMatch Match(ReadOnlySpan<byte> haystack) {
        SegmentMatch ret = NoSegmentMatch.Instance;

        var index = haystack.IndexOf(Segment.Pattern.AsSpan());
        if (index >= 0) {
            ret = new StringSegmentMatch {
                Segment = Segment,
                Index = index
            };
        }

        return ret;
    }
}
