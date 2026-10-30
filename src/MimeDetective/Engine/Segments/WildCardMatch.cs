using MimeDetective.Storage;

namespace MimeDetective.Engine;

/// <summary>
///     Represents a successful match against a <see cref="WildCardRule" />.
/// </summary>
public record WildCardMatch : PatternMatch {
    /// <summary>
    ///     The <see cref="WildCardRule" /> that was matched
    /// </summary>
    public required WildCardRule Rule { get; init; }

    /// <summary>
    ///     The location in <see cref="PatternMatch.Content" /> that the <see cref="Rule" /> was found at.
    /// </summary>
    public int Index { get; init; }
}
