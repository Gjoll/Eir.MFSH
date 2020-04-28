using System;
using System.Collections.Generic;
using System.Text;

namespace MFSH
{
    public class MacroDefinition : StackFrame
    {
        public String Name { get; set; }
        public List<String> Parameters = new List<string>();

        public MacroDefinition()
        {
        }
    }
}
