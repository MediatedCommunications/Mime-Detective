using MimeDetective.Storage;
using System;

namespace MimeDetective.Engine;

/// <summary>
///     An <see cref="IMatcher" /> that matches a <see cref="WildCardRule" /> against content.
/// </summary>
internal class WildCardMatcherBrute : WildCardMatcher {
    public WildCardMatcherBrute(WildCardRule segment) : base(segment) { }

    public override Match Match(ReadOnlySpan<byte> haystack) {
        Match ret = NoMatch.Instance;

        var index = haystack.IndexOf(Segment.Value.AsSpan());
        if (index >= 0) {
            ret = new WildCardMatch {
                Rule = Segment,
                Index = index,
            };
        }

        return ret;
    }
}
