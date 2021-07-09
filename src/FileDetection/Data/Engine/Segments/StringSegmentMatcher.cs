
using FileDetection.Storage;

namespace FileDetection.Engine
{

    static internal class StringSegmentMatcher
    {
        public static ISegmentMatcher Create(StringSegment Segment)
        {
            return new StringSegmentMatcherBrute(Segment);
        }
    }
}
