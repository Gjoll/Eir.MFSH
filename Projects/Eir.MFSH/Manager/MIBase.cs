using System;
using System.Collections.Generic;
using System.Text;

namespace Eir.MFSH
{
    public class MIBase
    {
        public Int32 LineNumber { get; }
        public String SourceFile { get; }

        public MIBase(String sourceFile,
            Int32 lineNumber)
        {
            this.LineNumber = lineNumber;
            this.SourceFile = sourceFile;
        }
    }
}
