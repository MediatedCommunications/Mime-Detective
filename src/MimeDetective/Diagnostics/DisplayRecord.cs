using System.Diagnostics;

namespace MimeDetective.Diagnostics;

[DebuggerDisplay(DisplayBase.GetDebuggerDisplay)]
public record DisplayRecord : IGetDebuggerDisplay {
    string? IGetDebuggerDisplay.GetDebuggerDisplay() {
        return GetDebuggerDisplay();
    }

    public virtual string? GetDebuggerDisplay() {
        return ToString();
    }
}
