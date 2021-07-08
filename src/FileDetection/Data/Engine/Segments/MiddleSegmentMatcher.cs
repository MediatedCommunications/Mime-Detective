using FileDetection.Data.Engine;
using System.Collections.Immutable;

namespace FileDetection.Data.Engine
{

    /// <summary>
    /// An <see cref="ISegmentMatcher"/> that matches a <see cref="MiddleSegment"/> against content.
    /// </summary>
    public record MiddleSegmentMatcher : ISegmentMatcher
    {
        public MiddleSegment Segment { get; init; } = MiddleSegment.None;

        public SegmentMatch Match(ImmutableArray<byte> Haystack)
        {
            SegmentMatch ret = NoSegmentMatch.Instance;

            for (int i = 0; i <= Haystack.Length - Segment.Pattern.Length; i++)
            {
                if (Match(Haystack, Segment.Pattern, i))
                {
                    ret = new MiddleSegmentMatch()
                    {
                        Segment = Segment,
                        Location = i..(i + Segment.Pattern.Length)
                    };
                    break;
                }
            }

            return ret;
        }

        static bool Match(ImmutableArray<byte> Haystack, ImmutableArray<byte> Needle, int start)
        {
            var ret = true;

            for (int i = 0; i < Needle.Length; i++)
            {
                if (Needle[i] != Haystack[i + start])
                {
                    ret = false;
                    break;
                }
            }
                
            return ret;
        
        }

    }
}
