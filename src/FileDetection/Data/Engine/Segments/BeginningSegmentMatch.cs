using FileDetection.Data.Engine;
using System.Collections.Immutable;

namespace FileDetection.Data.Engine
{
    /// <summary>
    /// Represents a successful match against a <see cref="BeginningSegment"/>.
    /// </summary>
    public record BeginningSegmentMatch : PatternSegmentMatch
    {
        /// <summary>
        /// The <see cref="BeginningSegment"/> that was matched
        /// </summary>
        public BeginningSegment Segment { get; init; } = BeginningSegment.None;
    }
}
