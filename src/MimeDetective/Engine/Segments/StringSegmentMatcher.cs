
using MimeDetective.Storage;
using System.Collections.Immutable;

namespace MimeDetective.Engine
{

    public abstract class StringSegmentMatcher : ISegmentMatcher {
        public StringSegment Segment { get; }

        public StringSegmentMatcher(StringSegment Segment) {
            this.Segment = Segment;
        }

        public abstract SegmentMatch Match(ImmutableArray<byte> Content);


        public static StringSegmentMatcher Create(StringSegment Segment)
        {
            return CreateAdvanced(Segment);
        }

        public static StringSegmentMatcher CreateBasic(StringSegment Segment) {
            return new StringSegmentMatcherBrute(Segment);
        }

        public static StringSegmentMatcher CreateAdvanced(StringSegment Segment) {
            return new StringSegmentMatcherBoyerMoore(Segment);
        }

        
    }
}
