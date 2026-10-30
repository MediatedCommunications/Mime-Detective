namespace MimeDetective.Engine;

/// <summary>
///     A singleton instance that indicates that a match was not successful.
/// </summary>
public record NoMatch : Match {
    public static NoMatch Instance { get; } = new();

    private NoMatch() { }
}
