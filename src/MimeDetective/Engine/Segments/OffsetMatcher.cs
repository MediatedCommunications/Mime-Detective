using MimeDetective.Diagnostics;
using MimeDetective.Storage;
using System;

namespace MimeDetective.Engine;

/// <summary>
///     An <see cref="IMatcher" /> that matches a <see cref="OffsetRule" /> against content.
/// </summary>
internal class OffsetMatcher : DisplayClass, IMatcher {
    public OffsetRule Segment { get; }

    public OffsetMatcher(OffsetRule segment) {
        Segment = segment;
    }

    public Match Match(ReadOnlySpan<byte> content) {
        Match ret = NoMatch.Instance;

        var matches = true;
        matches &= content.Length >= Segment.ExclusiveEnd();

        if (matches) {
            for (int patternIndex = 0, contentIndex = Segment.Start;
                contentIndex < Segment.ExclusiveEnd();
                patternIndex++, contentIndex++) {
                if (Segment.Value[patternIndex] != content[contentIndex]) {
                    matches = false;
                    break;
                }
            }
        }

        if (matches) {
            ret = new OffsetMatch { Rule = Segment, };
        }

        return ret;
    }

    public override string? GetDebuggerDisplay() {
        return Segment.GetDebuggerDisplay();
    }

    public static OffsetMatcher Create(OffsetRule segment) {
        return new(segment);
    }
}
