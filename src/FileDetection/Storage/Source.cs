using FileDetection.Diagnostics;

namespace FileDetection.Storage
{
    public record Source : DisplayRecord
    {
        public long? Files { get; init; }

        public override string? GetDebuggerDisplay()
        {
            return $@"{Files?.ToString() ?? "???"} Files";
        }

    }
}
