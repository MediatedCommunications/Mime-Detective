using System.Collections.Immutable;

namespace FileDetection.Data.Engine
{
    public static class SegmentExtensions
    {
        /// <summary>
        /// Match a <see cref="BeginningSegment"/> against byte content.
        /// </summary>
        /// <param name="This"></param>
        /// <param name="Content"></param>
        /// <returns></returns>
        public static SegmentMatch GetMatch(this BeginningSegment This, ImmutableArray<byte> Content)
        {
            var Matcher = new BeginningSegmentMatcher()
            {
                Segment = This,
            };

            var ret = Matcher.Match(Content);

            return ret;
        }

        /// <summary>
        /// Match a <see cref="MiddleSegment"/> against byte content.
        /// </summary>
        /// <param name="This"></param>
        /// <param name="Content"></param>
        /// <returns></returns>
        public static SegmentMatch GetMatch(this MiddleSegment This, ImmutableArray<byte> Content)
        {
            var Matcher = new MiddleSegmentMatcher()
            {
                Segment = This,
            };

            var ret = Matcher.Match(Content);

            return ret;
        }

    }

}
