namespace MimeDetective.Engine;

/// <summary>
///     A singleton instance that indicates that a match was not successful.
/// </summary>
public record NoSegmentMatch : SegmentMatch {
    public static NoSegmentMatch Instance { get; } = new();

    private NoSegmentMatch() { }
}
