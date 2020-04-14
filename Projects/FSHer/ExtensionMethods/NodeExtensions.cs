using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eir.FSHer
{
    public static class NodeExtensions
    {
        public static NodeRule IsRule(this NodeBase item,
            String ruleName)
        {
            NodeRule retVal = item as NodeRule;
            if (retVal == null)
                throw new Exception($"Item is not a NodeRule");
            if (retVal.RuleName != ruleName)
                throw new Exception($"NodeRule has wrong rule name {retVal.RuleName}. Expected {ruleName}");
            return retVal;
        }
    }
}
