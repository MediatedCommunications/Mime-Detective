using FileDetection.Data.Engine;

namespace FileDetection.Data.Engine
{

    static internal class StringSegmentMatcher
    {
        public static ISegmentMatcher Create(StringSegment Segment)
        {
            return new StringSegmentMatcherBrute(Segment);
        }
    }
}
