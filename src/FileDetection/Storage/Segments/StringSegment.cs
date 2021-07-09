namespace FileDetection.Storage
{
    /// <summary>
    /// Represents a <see cref="Segment"/> that can occur anywhere in the content.
    /// </summary>
    public record StringSegment : PatternSegment
    {
        public static StringSegment None { get; } = new();
    }
}
