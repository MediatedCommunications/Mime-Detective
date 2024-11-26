using MimeDetective.Storage;

namespace MimeDetective.Engine;

/// <summary>
/// Represents a successful match against a <see cref="PrefixSegment"/>.
/// </summary>
public record PrefixSegmentMatch : PatternSegmentMatch {
    public override string? GetDebuggerDisplay() {
        return Segment.GetDebuggerDisplay();
    }

    /// <summary>
    /// The <see cref="PrefixSegment"/> that was matched
    /// </summary>
    public PrefixSegment Segment { get; init; } = PrefixSegment.None;
}