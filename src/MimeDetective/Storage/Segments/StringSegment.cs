using System.Collections.Generic;
using System.Collections.Immutable;

namespace MimeDetective.Storage {
    /// <summary>
    /// Represents a <see cref="Segment"/> that can occur anywhere in the content.
    /// </summary>
    public class StringSegment : PatternSegment
    {
        public static StringSegment None { get; } = new();

        public static StringSegment Create(string Text, bool ApostropheIsNull = true) {
            var ret = new Storage.StringSegment() {
                Pattern = BytesFromText(Text, ApostropheIsNull),
            };
            return ret;

        }

        public static StringSegment Create(IEnumerable<byte> Bytes) {
            return new StringSegment() {
                Pattern = Bytes.ToImmutableArray(),
            };
        }
    }
}
