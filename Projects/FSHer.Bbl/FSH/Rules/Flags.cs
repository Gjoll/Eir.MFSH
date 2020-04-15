using System;
using System.Collections.Generic;
using System.Text;

namespace FSHer.Bbl.FSH
{
    [Flags]
    public enum Flags
    {
        MOD = 0x0001,
        MS = 0x0002,
        SU = 0x0004,
        TU = 0x0008,
        NORMATIVE = 0x0010,
        DRAFT = 0x0020
    };
}
