using FileDetection.Storage;
using System.Collections.Generic;
using System.Linq;

namespace FileDetection.Storage
{
    public static class PrefixSegmentExtensions
    {

        /// <summary>
        /// The exclusive upper bound of the <see cref="PrefixSegment"/>'s <see cref="Segment.Content"/>
        /// </summary>
        /// <param name="This"></param>
        /// <returns></returns>
        public static int End(this PrefixSegment This)
        {
            return This.Start + This.Pattern.Length;
        }

        public static byte GetPatternAt(this PrefixSegment This, int Index)
        {
            var ret = This.Pattern[Index - This.Start];

            return ret;
        } 

        public static byte? TryGetPatternAt(this PrefixSegment? This, int Index)
        {
            var ret = default(byte?);

            if(TryGetPatternAt(This, Index, out var V1))
            {
                ret = V1;
            }

            return ret;
        }

        public static bool TryGetPatternAt(this PrefixSegment? This, int Index, out byte Value)
        {
            var ret = false;

            Value = default;

            if (This is { } && Index >= This.Start && Index < This.End()) {
                Value = This.GetPatternAt(Index);
                ret = true;
            }

            return ret;
        }


    }
}
