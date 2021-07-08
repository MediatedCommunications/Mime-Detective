using FileDetection.Data.Engine;
using System.Collections.Immutable;
using System.Linq;

namespace FileDetection.Data.Engine
{

    /// <summary>
    /// An <see cref="ISegmentMatcher"/> that matches a <see cref="BeginningSegment"/> against content.
    /// </summary>
    public record BeginningSegmentMatcher : ISegmentMatcher
    {
        public BeginningSegment Segment { get; init; } = new();

        public SegmentMatch Match(ImmutableArray<byte> Content)
        {
            SegmentMatch ret = NoSegmentMatch.Instance;

            var Matches = true
                && Content.Length >= Segment.End()
                && Enumerable.SequenceEqual(
                    Content
                        .Skip(Segment.Start)
                        .Take(Segment.Pattern.Length)
                        ,
                    Segment.Pattern
                    )
                ;

            if (Matches)
            {
                ret = new BeginningSegmentMatch()
                {
                    Segment = Segment,
                };
            }



            return ret;
        }
    }
}
