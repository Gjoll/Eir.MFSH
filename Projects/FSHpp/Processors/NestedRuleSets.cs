using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSHpp.Nodes;

namespace FSHpp.Processors
{
    /// <summary>
    /// Processes Nested RuleSets
    /// </summary>
    class NestedRuleSets : ProcessorBase
    {
        List<NodeRule> ruleSets;

        public NestedRuleSets(FSHpp fshPp) : base(fshPp)
        {
        }

        /// <summary>
        /// Return rule set with indicated name if it has been
        /// processed (has no unexpanded macros) in it.
        /// Otherwise return null.
        NodeRule GetProcessedRuleSet(String macroName)
        {
            if (this.FSHpp.RuleSetDict.TryGetValue(macroName, out NodeRule retVal) == false)
                throw new Exception($"Macro {macroName} not found");

            // If rule set still in list, then it has not been expanded yet.
            if (this.ruleSets.Contains(retVal) == true)
                return null;
            return retVal;
        }

        bool IsMacroCall(NodeBase node, out String macroName)
        {
            macroName = null;
            NodeRule rule = node as NodeRule;
            if (rule == null)
                return false;
            if (rule.RuleName != FSHListener.SdRuleStr)
                return false;
            NodeRule macroNode = rule
                    .ChildNodes
                    .Rules()
                    .WithRuleName(FSHListener.MacroRuleStr)
                    .FirstOrDefault()
                    ;
            
            if (macroNode == null)
                return false;

            macroName = macroNode.Name;
            return true;
        }

        public bool ProcessRuleSet(NodeRule rs)
        {
            List<NodeBase> nodes = new List<NodeBase>();

            Int32 i = 0;

            while (i < rs.ChildNodes.Count)
            {
                NodeBase child = rs.ChildNodes[i];
                if (IsMacroCall(child, out String macroName))
                {
                    NodeRule ruleSet = this.GetProcessedRuleSet(macroName);
                    if (ruleSet == null)
                        return false;
                    nodes.Add(new NodeComment($"\n  // Start Macro {macroName}"));
                    nodes.AddRange(ruleSet.ChildNodes.Rules().WithRuleName(FSHListener.SdRuleStr));
                    nodes.Add(new NodeComment($"\n  // End Macro {macroName}"));
                }
                else
                {
                    nodes.Add(child);
                }

                i += 1;
            }

            rs.ChildNodes = nodes;
            return true;
        }

        public override void Process()
        {
            this.ruleSets = this.FSHpp.RuleSetDict.Values.ToList();

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
