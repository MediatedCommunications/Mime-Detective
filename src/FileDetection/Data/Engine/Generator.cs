using FileDetection.Data.Diagnostics;

namespace FileDetection.Data.Engine
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
