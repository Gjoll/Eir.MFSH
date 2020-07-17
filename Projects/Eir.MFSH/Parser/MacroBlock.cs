using System;
using System.Collections.Generic;
using System.Text;
using Eir.MFSH;

namespace Eir.MFSH.Parser
{
    public class MacroBlock : ParseBlock
    {
        public MIMacro Macro { get; }
        public String FragmentBase { get; set; }

        public MacroBlock(String sourceFile,
            Int32 lineNumber,
            String macroName,
            IEnumerable<String> parameters)
        {
            this.Macro = new MIMacro(sourceFile, lineNumber, macroName, parameters);
            // We want all items parsed to go into Macro.items.
            this.Items = this.Macro.Items;
        }
}
}
