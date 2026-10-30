using MimeDetective.Storage;
using System;

namespace MimeDetective.Engine;

/// <summary>
///     An <see cref="IMatcher" /> that matches a <see cref="WildCardRule" /> against content.
/// </summary>
internal class WildCardMatcherBoyerMoore : WildCardMatcher {
    private WildCardMatcherBoyerMooreProvider Bm { get; }

    public WildCardMatcherBoyerMoore(WildCardRule segment) : base(segment) {
        Bm = new(segment.Value);
    }

    public override Match Match(ReadOnlySpan<byte> haystack) {
        Match ret = NoMatch.Instance;

        if (Bm.SearchFirst(haystack) is { } i) {
            ret = new WildCardMatch {
                Rule = Segment,
                Index = i,
            };
        }


        return ret;
    }
}
