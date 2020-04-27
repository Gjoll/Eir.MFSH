using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MFSH
{
    public class VariablesStack
    {
        List<VariablesBlock> variableBlocks = new List<VariablesBlock>();
        public VariablesBlock Current => this.variableBlocks[this.variableBlocks.Count - 1];

        public VariablesStack()
        {
            new VariablesBlock("base", this);
        }

        public String StackTrace()
        {
            StringBuilder sb = new StringBuilder();
            for (Int32 i = this.variableBlocks.Count - 1; i >= 0; i--)
            {
                if (sb.Length > 0)
                    sb.Append("/");
                sb.Append(this.variableBlocks[i].Name);
            }

            return sb.ToString();
        }

        public void Push(VariablesBlock variablesBlock)
        {
            this.variableBlocks.Add(variablesBlock);
        }

        public void Pop()
        {
            this.variableBlocks.RemoveAt(this.variableBlocks.Count - 1);
        }

        /// <summary>
        /// Use each variable block, starting with the most recent,
        /// and replace the text with the variables.
        /// </summary>
        public String ReplaceText(String text)
        {
            for (Int32 i = this.variableBlocks.Count - 1; i >= 0; i--)
            {
                VariablesBlock vb = this.variableBlocks[i];
                text = vb.ReplaceText(text);
            }
            return text;
        }
    }
}
