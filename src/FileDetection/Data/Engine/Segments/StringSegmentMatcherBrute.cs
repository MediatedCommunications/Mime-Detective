using System.Collections.Immutable;

namespace FileDetection.Data.Engine
{
    /// <summary>
    /// An <see cref="ISegmentMatcher"/> that matches a <see cref="StringSegment"/> against content.
    /// </summary>
    internal class StringSegmentMatcherBrute : ISegmentMatcher
    {
        public StringSegmentMatcherBrute(StringSegment Segment)
        {
            this.Segment = Segment;
        }

        public StringSegment Segment { get; }

        public SegmentMatch Match(ImmutableArray<byte> Haystack)
        {
            SegmentMatch ret = NoSegmentMatch.Instance;

            for (var i = 0; i <= Haystack.Length - Segment.Pattern.Length; i++)
            {
                if (Match(Haystack, Segment.Pattern, i))
                {
                    ret = new StringSegmentMatch()
                    {
                        Segment = Segment,
                        Location = i..(i + Segment.Pattern.Length)
                    };
                    break;
                }
            }

            return ret;
        }

        private static bool Match(ImmutableArray<byte> Haystack, ImmutableArray<byte> Needle, int start)
        {
            var ret = true;

            for (var i = 0; i < Needle.Length; i++)
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
