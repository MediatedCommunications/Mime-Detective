using MimeDetective.Diagnostics;

namespace MimeDetective.Storage;

public record Generator : DisplayRecord {
    public string? Application { get; init; }

    public override string? GetDebuggerDisplay() {
        return $@"{Application}";
    }
}
