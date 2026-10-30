using MimeDetective.Diagnostics;
using System.Collections.Immutable;

namespace MimeDetective.Storage {
    public record Signature : DisplayRecord
    {
        public ImmutableArray<OffsetRule> Prefix { get; init; } = [];
        public ImmutableArray<WildCardRule> Strings { get; init; } = [];

        public override string? GetDebuggerDisplay()
        {
            return $@"{Prefix.Length} Prefixes /// {Strings.Length} Strings";
        }
    }

}
