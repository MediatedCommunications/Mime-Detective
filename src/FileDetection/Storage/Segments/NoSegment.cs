namespace FileDetection.Storage
{

    /// <summary>
    /// A singleton instance that represents a null segment.
    /// </summary>
    internal record NoSegment : Segment
    {
        private NoSegment() { }

        public static NoSegment Instance { get; } = new();
    }
}
