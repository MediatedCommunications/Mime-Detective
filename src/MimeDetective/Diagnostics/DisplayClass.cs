using System.Diagnostics;

namespace MimeDetective.Diagnostics;

[DebuggerDisplay(DisplayBase.GetDebuggerDisplay)]
public class DisplayClass : IGetDebuggerDisplay {
    string? IGetDebuggerDisplay.GetDebuggerDisplay() {
        return GetDebuggerDisplay();
    }

    public virtual string? GetDebuggerDisplay() {
        return ToString();
    }
}
