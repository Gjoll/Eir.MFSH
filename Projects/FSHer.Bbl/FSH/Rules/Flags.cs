using System;
using System.Collections.Generic;
using System.Text;

namespace FSHer.Bbl.FSH
{
    [Flags]
    public enum Flags
    {
        None = 0x0000,
        MOD = 0x0001,
        MS = 0x0002,
        SU = 0x0004,
        TU = 0x0008,
        NORMATIVE = 0x0010,
        DRAFT = 0x0020
    };

    public static class FlagHelper
    {
        public static String ToFsh(this Flags flags)
        {
            StringBuilder sbFlag = new StringBuilder();
            if ((flags & Flags.MOD) == Flags.MOD)
                sbFlag.Append(" MOD");
            if ((flags & Flags.MS) == Flags.MS)
                sbFlag.Append(" MS");
            if ((flags & Flags.SU) == Flags.SU)
                sbFlag.Append(" SU");
            if ((flags & Flags.TU) == Flags.TU)
                sbFlag.Append(" TU");
            if ((flags & Flags.NORMATIVE) == Flags.NORMATIVE)
                sbFlag.Append(" NORMATIVE");
            if ((flags & Flags.DRAFT) == Flags.DRAFT)
                sbFlag.Append(" DRAFT");
            return sbFlag.ToString();
        }
    }
}
