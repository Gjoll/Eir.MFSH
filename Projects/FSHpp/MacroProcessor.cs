using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp
{
    public class MacroProcessor
    {
        private FSHpp pp;

        /// <summary>
        /// Dictionary of rule sets that have had any macro's in them
        /// expended.
        /// </summary>
        Dictionary<String, NodeRule> ruleSetDict = new Dictionary<string, NodeRule>();

        public MacroProcessor(FSHpp pp)
        {
            this.pp = pp;
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
                        NodeRule macroRefNode = (NodeRule) child;
                        if (this.ruleSetDict.TryGetValue(macroRefNode.Name, out NodeRule macroNode) == false)
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

        public void Process()
        {
            List<NodeRule> ruleSets = this.pp.ruleSetDict.Values.ToList();

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
                        ruleSetDict.Add(rs.Name, rs);
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
