using MimeDetective.Diagnostics;

namespace MimeDetective.Storage {
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
            return $@"{File.GetDebuggerDisplay()}: {Signature.GetDebuggerDisplay()} [{Meta?.Id}]";
        }

    }

}
