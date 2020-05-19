using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Eir.MFSH
{
    [DebuggerDisplay("Text '{Line}'")]
    public class MIText : MIBase
    {
        public String Line { get; set; }

    }
}
