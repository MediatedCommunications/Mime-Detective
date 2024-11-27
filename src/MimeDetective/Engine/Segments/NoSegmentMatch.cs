namespace MimeDetective.Engine;

/// <summary>
/// A singleton instance that indicates that a match was not successful.
/// </summary>
public record NoSegmentMatch : SegmentMatch {
    private NoSegmentMatch() { }
    public static NoSegmentMatch Instance { get; } = new();
}