
using MimeDetective.Diagnostics;
using MimeDetective.Storage;
using System;

namespace MimeDetective.Engine
{

    public abstract class StringSegmentMatcher : DisplayClass, ISegmentMatcher {
        public override string? GetDebuggerDisplay() {
            return Segment.GetDebuggerDisplay();
        }

        public StringSegment Segment { get; }

        public StringSegmentMatcher(StringSegment Segment) {
            this.Segment = Segment;
        }

        public abstract SegmentMatch Match(ReadOnlySpan<byte> Content);


        public static StringSegmentMatcher Create(StringSegment Segment)
        {
            return CreateHighSpeed(Segment);
        }

        public static StringSegmentMatcher CreateLowMemory(StringSegment Segment) {
            return new StringSegmentMatcherBrute(Segment);
        }

        public static StringSegmentMatcher CreateHighSpeed(StringSegment Segment) {
            return new StringSegmentMatcherBoyerMoore(Segment);
        }

        
    }
}
