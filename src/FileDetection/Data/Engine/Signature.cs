using FileDetection.Data.Diagnostics;
using FileDetection.Data.Engine;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace FileDetection.Data.Engine
{
    public record Signature : DisplayRecord
    {
        public ImmutableArray<PrefixSegment> Prefix { get; init; } = ImmutableArray<PrefixSegment>.Empty;
        public ImmutableArray<StringSegment> Strings { get; init; } = ImmutableArray<StringSegment>.Empty;

        public override string? GetDebuggerDisplay()
        {
            return $@"{Prefix.Length} Prefixes /// {Strings.Length} Strings";
        }
    }

}
