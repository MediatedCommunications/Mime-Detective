using FileDetection.Data.Engine;
using System.Collections.Immutable;

namespace FileDetection.Data.Engine
{
    /// <summary>
    /// Represents a successful match against a <see cref="PrefixSegment"/>.
    /// </summary>
    public record PrefixSegmentMatch : PatternSegmentMatch
    {
        /// <summary>
        /// The <see cref="PrefixSegment"/> that was matched
        /// </summary>
        public PrefixSegment Segment { get; init; } = PrefixSegment.None;
    }
}
