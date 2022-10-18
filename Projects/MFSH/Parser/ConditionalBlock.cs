using System;
using System.Collections.Generic;
using System.Text;
using MFSH;

namespace MFSH.Parser
{
    public class ConditionalBlock : ParseBlock
    {
        public MIConditional Conditional { get; }

        public ConditionalBlock(String sourceFile,
            Int32 lineNumber) : base("conditional")
        {
            this.Conditional = new MIConditional(sourceFile, lineNumber);
        }

        public void AddCondition(MIConditional.Condition condition)
        {
            // We want all items parsed to go into Macro.items.
            this.Conditional.Conditions.Add(condition);
            this.Items = condition.Items;
        }
    }
}
