namespace FileDetection.Data.Engine
{
    /// <summary>
    /// Represents a <see cref="Segment"/> that can occur anywhere in the content.
    /// </summary>
    public record MiddleSegment : PatternSegment
    {
        public static MiddleSegment None { get; } = new();
    }
}
