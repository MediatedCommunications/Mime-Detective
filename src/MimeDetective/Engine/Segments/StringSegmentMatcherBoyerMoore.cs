using MimeDetective.Storage;
using System;

namespace MimeDetective.Engine;

/// <summary>
/// An <see cref="ISegmentMatcher"/> that matches a <see cref="StringSegment"/> against content.
/// </summary>
internal class StringSegmentMatcherBoyerMoore : StringSegmentMatcher {
    public StringSegmentMatcherBoyerMoore(StringSegment segment) : base(segment) {
        this.Bm = new StringSegmentMatcherBoyerMooreProvider(segment.Pattern);
    }

    private StringSegmentMatcherBoyerMooreProvider Bm { get; }

    public override SegmentMatch Match(ReadOnlySpan<byte> haystack) {
        SegmentMatch ret = NoSegmentMatch.Instance;

        if (this.Bm.SearchFirst(haystack) is { } i) {

            ret = new StringSegmentMatch {
                Segment = Segment,
                Index = i
            };
        }


        return ret;
    }

}