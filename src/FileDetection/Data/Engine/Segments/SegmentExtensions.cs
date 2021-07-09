using System.Collections.Immutable;

namespace FileDetection.Data.Engine
{
    static internal class SegmentExtensions
    {
        /// <summary>
        /// Match a <see cref="PrefixSegment"/> against byte content.
        /// </summary>
        /// <param name="This"></param>
        /// <param name="Content"></param>
        /// <returns></returns>
        public static SegmentMatch GetMatch(this PrefixSegment This, ImmutableArray<byte> Content)
        {
            var Matcher = new PrefixSegmentMatcher()
            {
                Segment = This,
            };

            var ret = Matcher.Match(Content);

            return ret;
        }

        /// <summary>
        /// Match a <see cref="StringSegment"/> against byte content.
        /// </summary>
        /// <param name="This"></param>
        /// <param name="Content"></param>
        /// <returns></returns>
        public static SegmentMatch GetMatch(this StringSegment This, ImmutableArray<byte> Content)
        {
            var Matcher = StringSegmentMatcher.Create(This);
            
            var ret = Matcher.Match(Content);

            return ret;
        }

    }

}
