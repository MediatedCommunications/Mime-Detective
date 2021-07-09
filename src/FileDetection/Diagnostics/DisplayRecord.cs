using System.Diagnostics;

namespace FileDetection.Diagnostics
{
    [DebuggerDisplay(DisplayBase.GetDebuggerDisplay)]
    public record DisplayRecord : IGetDebuggerDisplay
    {

        public virtual string? GetDebuggerDisplay()
        {
            return this.ToString();
        }

        string? IGetDebuggerDisplay.GetDebuggerDisplay()
        {
            return GetDebuggerDisplay();
        }
    }
}
