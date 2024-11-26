using MimeDetective.Diagnostics;
using System.Collections.Immutable;

namespace MimeDetective.Storage;

public record Signature : DisplayRecord {
    public ImmutableArray<PrefixSegment> Prefix { get; init; } = [];
    public ImmutableArray<StringSegment> Strings { get; init; } = [];

    public override string? GetDebuggerDisplay() {
        return $@"{Prefix.Length} Prefixes /// {Strings.Length} Strings";
    }
}