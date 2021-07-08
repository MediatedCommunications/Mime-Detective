using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDetection.Data.Diagnostics
{

    [DebuggerDisplay(DisplayBase.GetDebuggerDisplay)]
    public class DisplayClass : IGetDebuggerDisplay
    {

        public virtual string? GetDebuggerDisplay() {
            return this.ToString();
        }

        string? IGetDebuggerDisplay.GetDebuggerDisplay()
        {
            return GetDebuggerDisplay();
        }
    }
}
