namespace FileDetection.Data.Engine
{

    /// <summary>
    /// A singleton instance that represents a null segment.
    /// </summary>
    public record NoSegment : Segment
    {
        private NoSegment() { }

        public static NoSegment Instance { get; } = new();
    }
}
