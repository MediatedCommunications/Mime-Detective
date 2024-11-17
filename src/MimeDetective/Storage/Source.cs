using MimeDetective.Diagnostics;
using System.Globalization;

namespace MimeDetective.Storage
{
    public record Source : DisplayRecord
    {
        public long? Files { get; init; }

        public override string? GetDebuggerDisplay()
        {
            return $"{Files?.ToString(CultureInfo.CurrentCulture) ?? "???"} Files";
        }

    }
}
