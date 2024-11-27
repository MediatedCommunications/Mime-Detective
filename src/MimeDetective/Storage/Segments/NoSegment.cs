namespace MimeDetective.Storage;

/// <summary>
/// A singleton instance that represents a null segment.
/// </summary>
internal class NoSegment : Segment {
    private NoSegment() { }

    public static NoSegment Instance { get; } = new();
}