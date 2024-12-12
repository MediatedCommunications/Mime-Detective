using MimeDetective.Storage;
using System;

namespace MimeDetective.Engine;

/// <summary>
///     An <see cref="ISegmentMatcher" /> that matches a <see cref="StringSegment" /> against content.
/// </summary>
internal class StringSegmentMatcherBoyerMoore : StringSegmentMatcher {
    private StringSegmentMatcherBoyerMooreProvider Bm { get; }

    public StringSegmentMatcherBoyerMoore(StringSegment segment) : base(segment) {
        Bm = new(segment.Pattern);
    }

    public override SegmentMatch Match(ReadOnlySpan<byte> haystack) {
        SegmentMatch ret = NoSegmentMatch.Instance;

        if (Bm.SearchFirst(haystack) is { } i) {
            ret = new StringSegmentMatch {
                Segment = Segment,
                Index = i
            };
        }


        return ret;
    }
}
