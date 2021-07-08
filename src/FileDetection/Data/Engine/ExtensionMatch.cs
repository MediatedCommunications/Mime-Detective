using FileDetection.Data.Diagnostics;
using FileDetection.Data.Engine;
using System.Collections.Immutable;

namespace FileDetection.Data.Engine
{
    public record ExtensionMatch : DisplayRecord
    {
        public string Extension { get; init; } = string.Empty;
        public long Points { get; init; }
        public ImmutableArray<DefinitionMatch> Matches { get; init; } = ImmutableArray<DefinitionMatch>.Empty;

        public override string? GetDebuggerDisplay()
        {
            return $@"{Extension} ({Points} Points)";
        }

    }
}
