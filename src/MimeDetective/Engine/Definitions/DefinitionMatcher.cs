using MimeDetective.Diagnostics;
using MimeDetective.Storage;
using System.Collections.Immutable;

namespace MimeDetective.Engine;

internal class DefinitionMatcher : DisplayClass {

    public override string? GetDebuggerDisplay() {
        return Definition.GetDebuggerDisplay();
    }

    public DefinitionMatcher(Definition definition) {
        this.Definition = definition;
    }

    public Definition Definition { get; }
    public ImmutableArray<PrefixSegmentMatcher> Prefixes { get; init; } = ImmutableArray<PrefixSegmentMatcher>.Empty;
    public ImmutableArray<StringSegmentMatcher> Strings { get; init; } = ImmutableArray<StringSegmentMatcher>.Empty;

}