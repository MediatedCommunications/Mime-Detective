using FileDetection.Diagnostics;
using System.Collections.Immutable;
using System.Linq;

namespace FileDetection.Storage
{
    public record FileType : DisplayRecord
    {
        public string? Description { get; init; }
        public ImmutableArray<string> Extensions { get; init; } = ImmutableArray<string>.Empty;
        public string? MimeType { get; init; }

        public override string? GetDebuggerDisplay()
        {
            var Extension = string.Join(@"/", Extensions.Select(x => x.ToUpper()));

            return $@"{Extension} ({MimeType} /// {Description})";
        }
    }
}
