using MimeDetective.Storage;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MimeDetective.Storage {
    public static class PrefixSegmentExtensions
    {

        /// <summary>
        /// The exclusive upper bound of the <see cref="PrefixSegment"/>'s <see cref="Segment.Content"/>
        /// </summary>
        /// <param name="This"></param>
        /// <returns></returns>
        public static int ExclusiveEnd(this PrefixSegment This)
        {
            return This.Start + This.Pattern.Length;
        }

        public static byte?[] TryGetRange(this IEnumerable<PrefixSegment> This, int Index, int Length) {
            var ret = new byte?[Length];
            var Segments = This.OrderBy(x => x.Start).ToArray();
            var SegmentIndex = 0;

            for (var i = 0; i < Length; i++) {
                var ByteIndex = Index + i;

                while (SegmentIndex < Segments.Length && (ByteIndex < Segments[SegmentIndex].Start || ByteIndex >= Segments[SegmentIndex].ExclusiveEnd())) {
                    SegmentIndex += 1;
                }

                if(SegmentIndex < Segments.Length) {
                    ret[i] = Segments[SegmentIndex].TryGetPatternAt(ByteIndex);
                }

            }


            return ret;
        }

        public static byte? TryGetPatternAt(this PrefixSegment? This, int Index) {
            var ret = default(byte?);

            if (TryGetPatternAt(This, Index, out var V1)) {
                ret = V1;
            }

            return ret;
        }

        public static bool TryGetPatternAt(this PrefixSegment? This, int Index, out byte Value) {
            var ret = false;

            Value = default;

            if (This is { } && Index >= This.Start && Index < This.ExclusiveEnd()) {
                Value = This.GetPatternAt(Index);
                ret = true;
            }

            return ret;
        }

        public static byte GetPatternAt(this PrefixSegment This, int Index) {
            var ret = This.Pattern[Index - This.Start];

            return ret;
        }

        /// <summary>
        /// Break a Prefix segment into individual 1-byte segments.
        /// </summary>
        /// <param name="This"></param>
        /// <returns></returns>
        public static IEnumerable<PrefixSegment> Singularize(this PrefixSegment This) {
            for (var i = 0; i < This.Pattern.Length; i++) {
                var ret = PrefixSegment.Create(This.Start + i, This.Pattern[i]);

                yield return ret;
            }
        }

    }

}
