using FileDetection.Data.Engine;
using System;
using System.Collections.Immutable;

namespace FileDetection.Data.Engine
{

    /// <summary>
    /// Represents a successful match against a <see cref="MiddleSegment"/>.
    /// </summary>
    public record MiddleSegmentMatch : PatternSegmentMatch
    {
        /// <summary>
        /// The <see cref="MiddleSegment"/> that was matched
        /// </summary>
        public MiddleSegment Segment { get; init; } = MiddleSegment.None;

        /// <summary>
        /// The location in <see cref="PatternSegmentMatch.Content"/> that the <see cref="Segment"/> was found at.
        /// </summary>
        public Range Location { get; init; }
    }
}
