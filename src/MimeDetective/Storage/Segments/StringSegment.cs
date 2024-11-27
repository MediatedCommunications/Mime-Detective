using System.Collections.Generic;
using System.Collections.Immutable;

namespace MimeDetective.Storage;

/// <summary>
/// Represents a <see cref="Segment"/> that can occur anywhere in the content.
/// </summary>
public class StringSegment : PatternSegment {
    public static StringSegment None { get; } = new();

    public static StringSegment Create(string text, bool apostropheIsNull = true) {
        var ret = new Storage.StringSegment() {
            Pattern = BytesFromText(text, apostropheIsNull),
        };
        return ret;

    }

    public static StringSegment Create(IEnumerable<byte> bytes) {
        return new StringSegment() {
            Pattern = bytes.ToImmutableArray(),
        };
    }
}