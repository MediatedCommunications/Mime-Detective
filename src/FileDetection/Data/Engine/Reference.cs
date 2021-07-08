using FileDetection.Data.Diagnostics;

namespace FileDetection.Data.Engine
{
    public record Reference : DisplayRecord
    {
        public string? Text { get; init; }
        public string? Uri { get; init; }
    }
}
