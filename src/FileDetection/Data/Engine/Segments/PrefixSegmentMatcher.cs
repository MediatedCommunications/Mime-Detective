using FileDetection.Data.Engine;
using System.Collections.Immutable;
using System.Linq;

namespace FileDetection.Data.Engine
{

    /// <summary>
    /// An <see cref="ISegmentMatcher"/> that matches a <see cref="PrefixSegment"/> against content.
    /// </summary>
    internal class PrefixSegmentMatcher : ISegmentMatcher
    {
        public PrefixSegment Segment { get; init; } = new();

        public SegmentMatch Match(ImmutableArray<byte> Content)
        {
            SegmentMatch ret = NoSegmentMatch.Instance;

            var Matches = true;
            Matches &= Content.Length >= Segment.End();

            if (Matches)
            {
                for (int PatternIndex = 0, ContentIndex = Segment.Start; ContentIndex < Segment.End(); PatternIndex++, ContentIndex++)
                {
                    if(Segment.Pattern[PatternIndex] != Content[ContentIndex])
                    {
                        Matches = false;
                        break;
                    }
                }
            }

            if (Matches)
            {
                ret = new PrefixSegmentMatch()
                {
                    Segment = Segment,
                };
            }

            return ret;
        }
    }
}
