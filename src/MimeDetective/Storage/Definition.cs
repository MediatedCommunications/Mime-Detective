using MimeDetective.Diagnostics;

namespace MimeDetective.Storage;

/// <summary>
///     WildCard information used to identify a file.
/// </summary>
public record Definition : DisplayRecord {
    public FileType File { get; init; } = new();
    public Signature Signature { get; init; } = new();

    public Meta? Meta { get; init; }

    public override string? GetDebuggerDisplay() {

        var Prefix = string.Empty;
        {
            if(Meta?.Id is { } Id) {
                Prefix = $@"[{Id}] ";
            }

        }

        return $@"{Prefix}{File.GetDebuggerDisplay()}: {Signature.GetDebuggerDisplay()}";
    }
}
