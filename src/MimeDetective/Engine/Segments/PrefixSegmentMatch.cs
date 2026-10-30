using MimeDetective.Storage;

namespace MimeDetective.Engine;

/// <summary>
///     Represents a successful match against a <see cref="PrefixSegment" />.
/// </summary>
public record PrefixSegmentMatch : PatternSegmentMatch {
    /// <summary>
    ///     The <see cref="PrefixSegment" /> that was matched
    /// </summary>
    public required PrefixSegment Segment { get; init; }

    public override string? GetDebuggerDisplay() {
        return Segment.GetDebuggerDisplay();
    }
}
