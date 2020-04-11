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
    class Macros : ProcessorBase
    {
        /// <summary>
        /// New profile rule children nodes.
        /// </summary>
        List<NodeBase> nodes = new List<NodeBase>();

        private String profileName;

        public Macros(FSHpp fshPp) : base(fshPp)
        {
        }

        public override void Process()
        {
            IEnumerable<NodeRule> profiles = this.FSHpp.ProfileDict.Values;
            foreach (NodeRule profile in profiles)
            {
                this.nodes = new List<NodeBase>();
                this.profileName = profile.Name;
                this.Process(profile);
            }
        }

        /// <summary>
        /// Return rule set with indicated name if it has been
        /// processed (has no unexpanded macros) in it.
        /// Otherwise return null.
        bool ExpandMacro(String macroName)
        {
            const String fcn = "ExpandMacro";

            if (this.FSHpp.RuleSetDict.TryGetValue(macroName, out NodeRule ruleSet) == false)
            {
                this.FSHpp.ConversionError(this.GetType().Name, fcn, $"Profile: {profileName}, macro {macroName} not found.");
                return false;
            }

            this.nodes.Add(new NodeComment($"\n  // Start Macro {macroName}"));
            this.Process(ruleSet);
            this.nodes.Add(new NodeComment($"\n  // End Macro {macroName}"));
            return true;
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

        bool Process(NodeRule rule)
        {
            const String fcn = "Process";

            Int32 i = 0;

            while (i < rule.ChildNodes.Count)
            {
                NodeBase child = rule.ChildNodes[i];
                if (IsMacroCall(child, out String macroName))
                {
                    this.FSHpp.ConversionInfo(this.GetType().Name, fcn, $"Profile: {profileName}, expanding macro {macroName}");

                    if (this.ExpandMacro(macroName) == false)
                        return false;
                }
                else
                {
                    nodes.Add(child);
                }

                i += 1;
            }

            rule.ChildNodes = nodes;
            return true;
        }
    }
}
