
using MimeDetective.Diagnostics;
using MimeDetective.Storage;
using System;

namespace MimeDetective.Engine;

public abstract class StringSegmentMatcher : DisplayClass, ISegmentMatcher {
    public override string? GetDebuggerDisplay() {
        return Segment.GetDebuggerDisplay();
    }

    public StringSegment Segment { get; }

    public StringSegmentMatcher(StringSegment segment) {
        this.Segment = segment;
    }

    public abstract SegmentMatch Match(ReadOnlySpan<byte> content);


    public static StringSegmentMatcher Create(StringSegment segment) {
        return CreateHighSpeed(segment);
    }

    public static StringSegmentMatcher CreateLowMemory(StringSegment segment) {
        return new StringSegmentMatcherBrute(segment);
    }

    public static StringSegmentMatcher CreateHighSpeed(StringSegment segment) {
        return new StringSegmentMatcherBoyerMoore(segment);
    }


}