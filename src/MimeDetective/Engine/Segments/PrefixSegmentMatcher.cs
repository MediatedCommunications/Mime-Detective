using MimeDetective.Diagnostics;
using MimeDetective.Storage;
using System;

namespace MimeDetective.Engine;

/// <summary>
///     An <see cref="ISegmentMatcher" /> that matches a <see cref="PrefixSegment" /> against content.
/// </summary>
internal class PrefixSegmentMatcher : DisplayClass, ISegmentMatcher {
    public PrefixSegment Segment { get; }

    public PrefixSegmentMatcher(PrefixSegment segment) {
        Segment = segment;
    }

    public SegmentMatch Match(ReadOnlySpan<byte> content) {
        SegmentMatch ret = NoSegmentMatch.Instance;

        var matches = true;
        matches &= content.Length >= Segment.ExclusiveEnd();

        if (matches) {
            for (int patternIndex = 0, contentIndex = Segment.Start;
                contentIndex < Segment.ExclusiveEnd();
                patternIndex++, contentIndex++) {
                if (Segment.Pattern[patternIndex] != content[contentIndex]) {
                    matches = false;
                    break;
                }
            }
        }

        if (matches) {
            ret = new PrefixSegmentMatch { Segment = Segment };
        }

        return ret;
    }

    public override string? GetDebuggerDisplay() {
        return Segment.GetDebuggerDisplay();
    }

    public static PrefixSegmentMatcher Create(PrefixSegment segment) {
        return new(segment);
    }
}
