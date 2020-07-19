using System;
using System.Collections.Generic;
using System.Text;
using Eir.MFSH;

namespace Eir.MFSH.Parser
{
    public class MacroBlock : ParseBlock
    {
        public MIApplicable Item { get; }

        public MacroBlock(String name, MIApplicable item) : base(name)
        {
            this.Item = item;
            // We want all items parsed to go into Macro.items.
            this.Items = this.Item.Items;
        }

        //public MacroBlock(String sourceFile,
        //    Int32 lineNumber,
        //    String macroName,
        //    IEnumerable<String> parameters)
        //{
        //    this.Item = new MIMacro(sourceFile, lineNumber, macroName, parameters);
        //    // We want all items parsed to go into Macro.items.
        //    this.Items = this.Item.Items;
        //}
    }
}
