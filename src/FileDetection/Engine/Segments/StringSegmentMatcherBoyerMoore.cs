using FileDetection.Storage;
using System.Collections.Immutable;
using System.Linq;

namespace FileDetection.Engine
{
    /// <summary>
    /// An <see cref="ISegmentMatcher"/> that matches a <see cref="StringSegment"/> against content.
    /// </summary>
    internal class StringSegmentMatcherBoyerMoore : StringSegmentMatcher
    {
        public StringSegmentMatcherBoyerMoore(StringSegment Segment) : base(Segment)
        {
            this.BM = new StringSegmentMatcherBoyerMooreProvider(Segment.Pattern);
        }

        private StringSegmentMatcherBoyerMooreProvider BM { get; }

        public override SegmentMatch Match(ImmutableArray<byte> Haystack){
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
