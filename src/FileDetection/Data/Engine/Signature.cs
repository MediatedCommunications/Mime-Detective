using FileDetection.Data.Diagnostics;
using FileDetection.Data.Engine;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace FileDetection.Data.Engine
{
    public record Signature : DisplayRecord
    {
        public ImmutableArray<BeginningSegment> Beginning { get; init; } = ImmutableArray<BeginningSegment>.Empty;
        public ImmutableArray<MiddleSegment> Middle { get; init; } = ImmutableArray<MiddleSegment>.Empty;

        public override string? GetDebuggerDisplay()
        {
            return $@"{Beginning.Length} Front /// {Middle.Length} Any";
        }
    }

}
