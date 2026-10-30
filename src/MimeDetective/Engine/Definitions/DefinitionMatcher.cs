using MimeDetective.Diagnostics;
using MimeDetective.Storage;
using System.Collections.Immutable;

namespace MimeDetective.Engine {
    internal class DefinitionMatcher : DisplayClass {

        public override string? GetDebuggerDisplay() {
            return Definition.GetDebuggerDisplay();
        }

        public DefinitionMatcher(Definition Definition) {
            this.Definition = Definition;
        }

        public Definition Definition { get; }
        public ImmutableArray<OffsetMatcher> Prefixes { get; init; } = [];
        public ImmutableArray<WildCardMatcher> Strings { get; init; } = [];

    }
}
