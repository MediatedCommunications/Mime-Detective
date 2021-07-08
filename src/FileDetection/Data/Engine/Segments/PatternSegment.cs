using System;
using System.Collections.Immutable;
using System.Linq;

namespace FileDetection.Data.Engine
{
    /// <summary>
    /// The base class representing <see cref="Pattern"/> that exists in the target file.
    /// </summary>
    public abstract record PatternSegment : Segment
    {
        /// <summary>
        /// The <see cref="Pattern"/> that must exist in the target file.
        /// </summary>
        public ImmutableArray<byte> Pattern { get; init; } = ImmutableArray<byte>.Empty;

        public override string? GetDebuggerDisplay()
        {
            var Hex = System.Convert.ToHexString(Pattern.ToArray());
            var String = System.Text.Encoding.UTF8.GetString(Pattern.ToArray());
            String = String.Replace("\0", "'");

            return $@"{String} /// {Hex}";
        }

    }
}
