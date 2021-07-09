using FileDetection.Diagnostics;

namespace FileDetection.Storage
{
    public record Reference : DisplayRecord
    {
        public string? Text { get; init; }
        public string? Uri { get; init; }
    }
}
