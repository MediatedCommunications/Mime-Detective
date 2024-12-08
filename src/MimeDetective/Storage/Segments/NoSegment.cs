namespace MimeDetective.Storage;

/// <summary>
///     A singleton instance that represents a null segment.
/// </summary>
internal class NoSegment : Segment {
    public static NoSegment Instance { get; } = new();

    private NoSegment() { }
}
