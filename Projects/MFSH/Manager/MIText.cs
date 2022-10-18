using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MFSH
{
    [DebuggerDisplay("Text '{Line}'")]
    public class MIText : MIBase
    {
        public String Line { get; set; }

        public MIText(String sourceFile,
            Int32 lineNumber) : base(sourceFile, lineNumber)
        {
        }
    }
}
