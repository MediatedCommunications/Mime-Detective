namespace FileDetection.Data.Engine
{
    public static class BeginningSegmentExtensions
    {

        /// <summary>
        /// The exclusive upper bound of the <see cref="BeginningSegment"/>'s <see cref="Segment.Content"/>
        /// </summary>
        /// <param name="This"></param>
        /// <returns></returns>
        public static int End(this BeginningSegment This)
        {
            return This.Start + This.Pattern.Length;
        }

    }
}
