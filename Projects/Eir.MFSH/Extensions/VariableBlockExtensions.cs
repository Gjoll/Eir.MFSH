using System;
using System.Collections.Generic;
using System.Text;

namespace Eir.MFSH
{
    public static class VariableBlockExtensions
    {
        public static String ReplaceText(this IEnumerable<VariablesBlock> variableBlocks, String text)
        {
            foreach (VariablesBlock variablesBlock in variableBlocks)
                text = variablesBlock.ReplaceText(text);
            return text;
        }
    }
}
