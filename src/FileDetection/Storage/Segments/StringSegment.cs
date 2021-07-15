using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace FileDetection.Storage
{
    /// <summary>
    /// Represents a <see cref="Segment"/> that can occur anywhere in the content.
    /// </summary>
    public class StringSegment : PatternSegment
    {
        public static StringSegment None { get; } = new();

        public static StringSegment Create(string Text, bool ApostropheIsNull = true) {
            if (ApostropheIsNull) {
                Text = Text.Replace("'", "\0");
            }
            var Bytes = System.Text.Encoding.UTF8.GetBytes(Text);

            var ret = new Storage.StringSegment() {
                Pattern = Bytes.ToImmutableArray(),
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
