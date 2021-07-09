using System;
using System.Collections.Generic;
using System.Linq;

namespace FileDetection.Storage
{
    /// <summary>
    /// Represents <see cref="Segment.Content"/> that occurs near the beginning of a file.
    /// </summary>
    public record PrefixSegment : PatternSegment
    {
        public static PrefixSegment None { get; } = new();

        /// <summary>
        /// The offset of where <see cref="Segment.Content"/> is located, relative to the start of the file.
        /// </summary>
        public int Start { get; init; }

        public override string? GetDebuggerDisplay()
        {
            return $@"@{Start}: {base.GetDebuggerDisplay()}";
        }
    }
}
