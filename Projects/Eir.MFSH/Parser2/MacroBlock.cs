using System;
using System.Collections.Generic;
using System.Text;
using Eir.MFSH;

namespace Eir.MFSH
{
    public class MacroBlock : ParseBlock
    {
        public MIMacro Macro { get; } = new MIMacro();

        public MacroBlock()
        {
            // We want all items parsed to go into Macro.items.
            this.Items = this.Macro.Items;
        }
    }
}
