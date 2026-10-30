using System.Collections.Generic;
using System.Collections.Immutable;

namespace MimeDetective.Storage;

/// <summary>
///     Represents <see cref="Segment.Content" /> that occurs near the beginning of a file.
/// </summary>
public class PrefixSegment : PatternSegment {

    /// <summary>
    ///     The offset of where <see cref="Segment.Content" /> is located, relative to the start of the file.
    /// </summary>
    public int Start { get; init; }

    public static PrefixSegment Create(int start, params ImmutableArray<byte> bytes) {
        return new() {
            Start = start,
            Pattern = bytes
        };
    }

    public static PrefixSegment Create(int start, string hexString) {
        return Create(start, BytesFromHex(hexString));
    }

    public override string? GetDebuggerDisplay() {
        return $@"@{Start}: {base.GetDebuggerDisplay()}";
    }

    protected override int GetHashCodeInternal() {
        return Start ^ base.GetHashCodeInternal();
    }
}
