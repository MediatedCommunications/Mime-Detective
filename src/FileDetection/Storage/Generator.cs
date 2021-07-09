using FileDetection.Diagnostics;

namespace FileDetection.Storage
{
    public record Generator : DisplayRecord
    {
        public string? Application { get; init; }

        public override string? GetDebuggerDisplay()
        {
            return $@"{Application}";
        }
    }
}
