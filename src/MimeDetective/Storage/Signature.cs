using MimeDetective.Diagnostics;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MimeDetective.Storage
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
