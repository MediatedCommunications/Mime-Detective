using MimeDetective.Diagnostics;
using MimeDetective.Storage;
using System.Collections.Immutable;
using System.Linq;

namespace MimeDetective.Engine
{
    /// <summary>
    /// The results of matching a <see cref="Definition"/> against content.
    /// </summary>
    public record DefinitionMatch : DisplayRecord
    {
        public DefinitionMatch(Definition Definition)
        {
            this.Definition = Definition;
        }

        public Definition Definition { get; }

        /// <summary>
        /// How many <see cref="Points"/> this <see cref="DefinitionMatch"/> scored.  The higher this value, the better the <see cref="DefinitionMatch"/>.
        /// </summary>
        public long Points { get; init; }

        /// <summary>
        /// What <see cref="Percentage"/> of this <see cref="DefinitionMatch"/>'s <see cref="SegmentMatch"/>s were successful.
        /// </summary>
        public decimal Percentage { get; init; }

        
        public ImmutableDictionary<PrefixSegment, SegmentMatch> PrefixSegmentMatches { get; init; } = ImmutableDictionary<PrefixSegment, SegmentMatch>.Empty;
        public ImmutableDictionary<StringSegment, SegmentMatch> StringSegmentMatches { get; init; } = ImmutableDictionary<StringSegment, SegmentMatch>.Empty;

        public override string? GetDebuggerDisplay()
        {
            return $@"{Definition.File.Extensions.FirstOrDefault()} /// {Points} Points /// {Definition.File.Description}";
        }

    }

}
