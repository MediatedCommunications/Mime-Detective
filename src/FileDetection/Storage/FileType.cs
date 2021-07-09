using FileDetection.Diagnostics;
using System.Collections.Immutable;

namespace FileDetection.Storage
{
    public record FileType : DisplayRecord
    {
        public string? Description { get; init; }
        public ImmutableArray<string> Extensions { get; init; }
        public string? MimeType { get; init; }

        public override string? GetDebuggerDisplay()
        {
            var Extension = string.Join(@"/", Extensions);

            return $@"{Extension} ({MimeType} /// {Description})";
        }
    }
}
