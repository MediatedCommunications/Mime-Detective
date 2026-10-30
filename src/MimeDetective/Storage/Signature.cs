using MimeDetective.Diagnostics;
using System.Collections.Immutable;

namespace MimeDetective.Storage {
    public record Signature : DisplayRecord
    {

        public ImmutableArray<OffsetRule> OffsetRules { get; init; } = [];
        public ImmutableArray<WildCardRule> WildCardRules { get; init; } = [];

        public override string? GetDebuggerDisplay()
        {
            return $@"{OffsetRules.Length} Offset Rules /// {WildCardRules.Length} WildCard Rules";
        }
    }
    
}
