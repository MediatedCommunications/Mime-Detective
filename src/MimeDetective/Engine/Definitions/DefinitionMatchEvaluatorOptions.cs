using MimeDetective.Diagnostics;

namespace MimeDetective.Engine;

/// <summary>
///     Options for a <see cref="DefinitionMatchEvaluator" />
/// </summary>
public record DefinitionMatchEvaluatorOptions : DisplayRecord {
    /// <summary>
    ///     Analyze <see cref="MimeDetective.Storage.OffsetRule" />s
    /// </summary>
    public bool IncludeOffsetRules { get; init; } = true;

    /// <summary>
    ///     Analyze <see cref="MimeDetective.Storage.WildCardRule" />s
    /// </summary>
    public bool IncludeWildcardRules { get; init; } = true;

    /// <summary>
    ///     Should <see cref="DefinitionMatch" />es with <see cref="DefinitionMatch.Percentage" /> = 1 be returned.
    /// </summary>
    public bool IncludeMatchesComplete { get; init; } = true;

    /// <summary>
    ///     Should <see cref="DefinitionMatch" />es with <see cref="DefinitionMatch.Percentage" /> between 0..1 (exclusive) be
    ///     returned.
    /// </summary>
    public bool IncludeMatchesPartial { get; init; }

    /// <summary>
    ///     Should Non-Failed rules with <see cref="DefinitionMatch" />es with <see cref="DefinitionMatch.Percentage" /> at 0
    ///     be returned.
    /// </summary>
    public bool IncludeMatchesEmpty { get; init; }

    /// Should
    /// <see cref="DefinitionMatch" />
    /// es with
    /// <see cref="DefinitionMatch.Percentage" />
    /// = 0 be returned.
    public bool IncludeMatchesFailed { get; init; }
}
