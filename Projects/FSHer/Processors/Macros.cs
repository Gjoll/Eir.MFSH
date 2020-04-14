using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHer.Processors
{
    /// <summary>
    /// Processes Nested macros
    /// </summary>
    class Macros : ProcessorBase
    {
        private String profileName;

        public Macros(FSHer fsher) : base(fsher)
        {
        }

        public override void Process()
        {
            IEnumerable<NodeRule> profiles = this.FSHer.ProfileDict.Values;
            foreach (NodeRule profile in profiles)
            {
                List<NodeBase> nodes = new List<NodeBase>();
                this.profileName = profile.Name;
                this.Process(profile.ChildNodes, nodes);
                profile.ChildNodes = nodes;
            }
        }

        /// <summary>
        /// Return rule set with indicated name if it has been
        /// processed (has no unexpanded macros) in it.
        /// Otherwise return null.
        void ExpandMacro(String macroName,
            List<NodeBase> outNodes,
            List<String> parameterValues)
        {
            const String fcn = "ExpandMacro";

            if (this.FSHer.MacroDict.TryGetValue(macroName, out NodeRule macro) == false)
            {
                String msg = $"Profile: {profileName}, macro {macroName} not found.";
                outNodes.Add(new NodeComment($"Error: {msg}"));
                this.FSHer.ConversionError(this.GetType().Name, fcn, msg);
                return;
            }

            List<String> parameterNames = macro.Parameters;
            if (parameterValues.Count != parameterNames.Count)
            {
                String msg = $"Macro '{macroName}' expansion failed. Expected {parameterNames.Count} parameters, found {parameterValues.Count}";
                outNodes.Add(new NodeComment($"Error: {msg}"));
                this.FSHer.ConversionInfo(this.GetType().Name, fcn, msg);
                return;
            }

            outNodes.Add(new NodeComment($"\n  // Start Macro {macroName}"));
            this.Process<NodeRule>(macro.ChildNodes.Rules().ToList(), outNodes);
            outNodes.Add(new NodeComment($"\n  // End Macro {macroName}"));
            return;
        }

        bool IsMacroCall(NodeBase node,
            out String macroName, 
            out List<String> parameters)
        {
            macroName = null;
            parameters = null;

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
            parameters = macroNode.ChildNodes
                    .Tokens()
                    .WithTokenName("STRING")
                    .Select(s => s.TokenValue)
                    .ToList()
                ;
            return true;
        }

        bool Process<T>(List<T> macroNodes,
            List<NodeBase> outNodes)
        where T : NodeBase
        {
            const String fcn = "Process";

            Int32 i = 0;

            while (i < macroNodes.Count)
            {
                NodeBase child = macroNodes[i];
                if (IsMacroCall(child, out String macroName, out List<String> parameters))
                {
                    this.FSHer.ConversionInfo(this.GetType().Name, 
                        fcn, 
                        $"Profile: {profileName}, expanding macro {macroName}");

                    this.ExpandMacro(macroName, outNodes, parameters);
                }
                else
                {
                    outNodes.Add(child);
                }

                i += 1;
            }

            return true;
        }
    }
}
