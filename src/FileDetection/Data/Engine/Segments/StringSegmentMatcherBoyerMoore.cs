using FileDetection.Storage;
using System.Collections.Immutable;
using System.Linq;

namespace FileDetection.Engine
{
    /// <summary>
    /// An <see cref="ISegmentMatcher"/> that matches a <see cref="StringSegment"/> against content.
    /// </summary>
    public class StringSegmentMatcherBoyerMoore : ISegmentMatcher
    {
        public StringSegmentMatcherBoyerMoore(StringSegment Segment)
        {
            this.Segment = Segment;
            this.BM = new StringSegmentMatcherBoyerMooreProvider(Segment.Pattern);
        }

        public StringSegment Segment { get; }
        private StringSegmentMatcherBoyerMooreProvider BM { get; }

        public SegmentMatch Match(ImmutableArray<byte> Haystack){
            SegmentMatch ret = NoSegmentMatch.Instance;

            if (BM.Search(Haystack, false).ToArray() is { } V1 && V1.Length > 0) {
                
                var i = V1[0];

                ret = new StringSegmentMatch()
                {
                    Segment = Segment,
                    Location = i..(i + Segment.Pattern.Length)
                };
            }


            return ret;
        }

    }
}
