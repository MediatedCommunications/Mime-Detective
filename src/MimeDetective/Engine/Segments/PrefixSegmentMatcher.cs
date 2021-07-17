using MimeDetective.Storage;
using System.Collections.Immutable;
using System.Linq;

namespace MimeDetective.Engine
{

    /// <summary>
    /// An <see cref="ISegmentMatcher"/> that matches a <see cref="PrefixSegment"/> against content.
    /// </summary>
    internal class PrefixSegmentMatcher : ISegmentMatcher
    {
        public static PrefixSegmentMatcher Create(PrefixSegment Segment) {
            return new PrefixSegmentMatcher(Segment);
        }

        public PrefixSegmentMatcher(PrefixSegment Segment) {
            this.Segment = Segment;
        }

        public PrefixSegment Segment { get; }

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
