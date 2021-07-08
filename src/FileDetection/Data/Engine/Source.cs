using FileDetection.Data.Diagnostics;

namespace FileDetection.Data.Engine
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
