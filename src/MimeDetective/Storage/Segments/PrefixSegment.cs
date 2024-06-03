using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MimeDetective.Storage {
    /// <summary>
    /// Represents <see cref="Segment.Content"/> that occurs near the beginning of a file.
    /// </summary>
    public class PrefixSegment : PatternSegment
    {
        public static PrefixSegment None { get; } = new();

        /// <summary>
        /// The offset of where <see cref="Segment.Content"/> is located, relative to the start of the file.
        /// </summary>
        public int Start { get; init; }

        public static PrefixSegment Create(int Start, ImmutableArray<byte> Bytes) {
            return new PrefixSegment() {
                Start = Start,
                Pattern = Bytes,
            };
        }

        public static PrefixSegment Create(int Start, params byte[] Bytes) {
            return Create(Start, Bytes.ToImmutableArray());
        }

        public static PrefixSegment Create(int Start, IEnumerable<byte> Bytes) {
            return Create(Start, Bytes.ToImmutableArray());
        }

        public static PrefixSegment Create(int Start, string HexString) {
            return Create(Start, BytesFromHex(HexString));
        }

        public override string? GetDebuggerDisplay()
        {
            return $@"@{Start}: {base.GetDebuggerDisplay()}";
        }

        protected override int GetHashCodeInternal() {
            return Start ^ base.GetHashCodeInternal();
        }
    }
}
