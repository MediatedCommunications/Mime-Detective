using FileDetection.Storage;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace FileDetection.Storage {
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

    }

}
