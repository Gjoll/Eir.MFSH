using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp.Processors
{
    /// <summary>
    /// Processes Nested RuleSets
    /// </summary>
    class NestedRuleSets : ProcessorBase
    {
        public NestedRuleSets(FSHpp fshPp) : base(fshPp)
        {
        }


        public bool ProcessRuleSet(NodeRule rs)
        {
            List<NodeBase> nodes = new List<NodeBase>();
            for (Int32 i = 0; i < rs.ChildNodes.Count; i++)
            {
                NodeBase child = rs.ChildNodes[i];
                switch (child.NodeType.ToLower())
                {
                    case "macro":
                        NodeRule macroRefNode = (NodeRule)child;
                        if (this.FSHpp.RuleSetDict.TryGetValue(macroRefNode.Name, out NodeRule macroNode) == false)
                            return false;
                        nodes.AddRange(macroNode.ChildNodes);
                        break;

                    default:
                        nodes.Add(child);
                        break;
                }
            }

            rs.ChildNodes = nodes;
            return true;
        }

        public override void Process()
        {
            List<NodeRule> ruleSets = this.FSHpp.RuleSetDict.Values.ToList();

            while (ruleSets.Count > 0)
            {
                bool successFlag = false;
                Int32 i = 0;
                while (i < ruleSets.Count)
                {
                    NodeRule rs = ruleSets[i];
                    if (this.ProcessRuleSet(rs) == true)
                    {
                        successFlag = true;
                        ruleSets.RemoveAt(i);
                    }
                    else
                    {
                        i += 1;
                    }
                }

                if (successFlag == false)
                    throw new Exception("Unable to process macros (circular reference?)");
            }
        }
    }
}
