using MimeDetective.Storage;

namespace MimeDetective.Engine;

/// <summary>
///     Represents a successful match against a <see cref="OffsetRule" />.
/// </summary>
public record OffsetMatch : PatternMatch {
    /// <summary>
    ///     The <see cref="OffsetRule" /> that was matched
    /// </summary>
    public required OffsetRule Rule { get; init; }

    public override string? GetDebuggerDisplay() {
        return Rule.GetDebuggerDisplay();
    }
}
