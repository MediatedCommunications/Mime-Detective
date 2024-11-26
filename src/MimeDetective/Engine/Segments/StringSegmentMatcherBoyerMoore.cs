using MimeDetective.Storage;
using System;

namespace MimeDetective.Engine;

/// <summary>
/// An <see cref="ISegmentMatcher"/> that matches a <see cref="StringSegment"/> against content.
/// </summary>
internal class StringSegmentMatcherBoyerMoore : StringSegmentMatcher {
    public StringSegmentMatcherBoyerMoore(StringSegment Segment) : base(Segment) {
        this.BM = new StringSegmentMatcherBoyerMooreProvider(Segment.Pattern);
    }

    private StringSegmentMatcherBoyerMooreProvider BM { get; }

    public override SegmentMatch Match(ReadOnlySpan<byte> Haystack) {
        SegmentMatch ret = NoSegmentMatch.Instance;

        if (BM.SearchFirst(Haystack) is { } i) {

            ret = new StringSegmentMatch {
                Segment = Segment,
                Index = i
            };
        }


        return ret;
    }

}