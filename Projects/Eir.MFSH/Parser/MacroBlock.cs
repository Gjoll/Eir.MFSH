using System;
using System.Collections.Generic;
using System.Text;
using Eir.MFSH;

namespace Eir.MFSH.Parser
{
    public class MacroBlock : ParseBlock
    {
        public MIMacro Macro { get; }

        public MacroBlock(String sourceFile,
            Int32 lineNumber)
        {
            this.Macro = new MIMacro(sourceFile, lineNumber);
            // We want all items parsed to go into Macro.items.
            this.Items = this.Macro.Items;
        }
}
}
