using FileDetection.Data.Engine;
using System;
using System.Collections.Immutable;

namespace FileDetection.Data.Engine
{

    /// <summary>
    /// Represents a successful match against a <see cref="StringSegment"/>.
    /// </summary>
    public record StringSegmentMatch : PatternSegmentMatch
    {
        /// <summary>
        /// The <see cref="StringSegment"/> that was matched
        /// </summary>
        public StringSegment Segment { get; init; } = StringSegment.None;

        /// <summary>
        /// The location in <see cref="PatternSegmentMatch.Content"/> that the <see cref="Segment"/> was found at.
        /// </summary>
        public Range Location { get; init; }
    }
}
