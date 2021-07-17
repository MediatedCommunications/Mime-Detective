using MimeDetective.Diagnostics;

namespace MimeDetective.Storage
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
