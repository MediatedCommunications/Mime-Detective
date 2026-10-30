using MimeDetective.Diagnostics;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;

namespace MimeDetective.Storage;

/// <summary>
///     Represents <see cref="Rule.Content" /> that occurs near the beginning of a file.
/// </summary>
public class OffsetRule : ValueRule {

    /// <summary>
    ///     The offset of where <see cref="Rule.Content" /> is located, relative to the start of the file.
    /// </summary>
    public int Start { get; init; }

    public static OffsetRule Create(int start, params ImmutableArray<byte> bytes) {
        return new() {
            Start = start,
            Bytes = bytes,
        };
    }

    public static OffsetRule FromAscii(int Start, string Text) {
        return FromText(Start, Text, Encoding.ASCII);
    }

    public static OffsetRule FromText(int start, string Text, Encoding Encoding) {
        return Create(start, [.. Encoding.GetBytes(Text),]);
    }

    public override string? GetDebuggerDisplay() {
        return $@"@{Start}: {base.GetDebuggerDisplay()}";
    }

    protected override int GetHashCodeInternal() {
        return Start ^ base.GetHashCodeInternal();
    }
}
