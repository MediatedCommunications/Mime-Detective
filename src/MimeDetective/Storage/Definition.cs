using MimeDetective.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MimeDetective.Storage
{
    /// <summary>
    /// Contains information used to identiy a file.
    /// </summary>
    public record Definition : DisplayRecord
    {
        public FileType File { get; init; } = new();
        public Signature Signature { get; init; } = new();

        public Meta? Meta { get; init; }

        public override string? GetDebuggerDisplay()
        {
            return $@"[{Meta?.Id}] {File.GetDebuggerDisplay()}: {Signature.GetDebuggerDisplay()}";
        }

    }

}
