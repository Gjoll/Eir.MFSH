using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eir.FSHer.Processors
{
    /// <summary>
    /// Processes Nested macros
    /// </summary>
    class Macros : ProcessorBase
    {
        private String profileName;
        private String profileParent;

        public Macros(FSHer fsher) : base(fsher)
        {
        }

        String FindParent(List<NodeRule> metaData)
        {
            return metaData
                    .Children()
                    .Rules()
                    .WithRuleName(FSHListener.ParentStr)
                    .Children()
                    .Tokens()
                    .WithTokenName(FSHListener.SEQUENCEStr)
                    .FirstOrDefault()
                    ?.TokenValue
                ;
        }

        List<NodeRule> FindMetaData(NodeRule rule, String metaDataName)
        {
            return rule
                .ChildNodes
                .Rules()
                .WithRuleName(metaDataName)
                .ToList();
        }

        public override void Process()
        {
            IEnumerable<NodeRule> profiles = this.FSHer.ProfileDict.Values;
            foreach (NodeRule profile in profiles)
            {
                List<NodeRule> profileMetaData = FindMetaData(profile, FSHListener.SdMetadataStr);

                List<NodeBase> nodes = new List<NodeBase>();
                this.profileName = profile.Name;
                this.profileParent = FindParent(profileMetaData);

                this.Process(profile.ChildNodes, 
                    nodes, 
                    new List<string>(), 
                    new List<string>());
                profile.ChildNodes = nodes;
            }
        }

        /// <summary>
        /// Return rule set with indicated name if it has been
        /// processed (has no unexpanded macros) in it.
        /// Otherwise return null.
        void ExpandMacro(NodeRule macroNode,
            String macroName,
            List<NodeBase> outNodes,
            List<String> parameterValues)
        {
            const String fcn = "ExpandMacro";

            String Error(String msg)
            {
                String fullMsg = $"Error: File {macroNode.FileName},  line {macroNode.LineNum}\n{msg}";
                return fullMsg;
            }

            if (this.FSHer.MacroDict.TryGetValue(macroName, out NodeRule macro) == false)
            {
                String msg = Error($"macro {macroName} not found.");
                outNodes.Add(new NodeComment($"\n{msg}"));
                this.FSHer.ConversionError(this.GetType().Name, fcn, msg);
                return;
            }

            List<NodeRule> metaData = FindMetaData(macro, FSHListener.MacroDefMetadataStr);

            String parent = this.FindParent(metaData);
            if (String.IsNullOrEmpty(parent) == false)
            {
                if (String.Compare(this.profileParent, parent) != 0)
                {
                    String msg = Error($"Macro '{macroName}' expansion failed. Macro can only be applied to profiles derived from {parent}");
                    outNodes.Add(new NodeComment($"\n{msg}"));
                    this.FSHer.ConversionError(this.GetType().Name, fcn, msg);
                    return;
                }
            }
            List<String> parameterNames = macro.Strings;
            if (parameterValues.Count != parameterNames.Count)
            {
                String msg = Error($"Macro '{macroName}' expansion failed. Expected {parameterNames.Count} parameters, found {parameterValues.Count}");
                outNodes.Add(new NodeComment($"\n{msg}"));
                this.FSHer.ConversionError(this.GetType().Name, fcn, msg);
                return;
            }

            outNodes.Add(new NodeComment($"\n  // Start Macro {macroName}"));
            this.Process<NodeRule>(macro
                    .ChildNodes
                    .Rules()
                    .ExcludeRules(FSHListener.MacroDefMetadataStr)
                    .ToList(),
                outNodes,
                parameterNames,
                parameterValues);

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
            parameters = macroNode.Strings;
                ;
            return true;
        }

        bool Process<T>(List<T> macroNodes,
            List<NodeBase> outNodes,
            List<String> parameterNames,
            List<String> parameterValues)
            where T : NodeBase
        {
            String ReplaceParams(String input,
                List<String> parameterNames,
                List<String> parameterValues)
            {
                if (parameterNames.Count != parameterValues.Count)
                    throw new Exception($"Invalid parameter list. Sizes do not match.");

                for (Int32 i = 0; i < parameterNames.Count; i++)
                {
                    input = input.Replace(parameterNames[i], parameterValues[i]);
                }

                return input;
            }

            const String fcn = "Process";

            Int32 i = 0;

            while (i < macroNodes.Count)
            {
                // We need to clone child because we are modifying it.
                NodeBase child = macroNodes[i].Clone();
                switch (child)
                {
                    case NodeRule rule:
                        if (IsMacroCall(rule, out String macroName, out List<String> parameters))
                        {
                            this.FSHer.ConversionInfo(this.GetType().Name,
                                fcn,
                                $"Profile: {profileName}, expanding macro {macroName}");
                            this.ExpandMacro(rule, macroName, outNodes, parameters);
                        }
                        else
                        {
                            List<NodeBase> cNodes = new List<NodeBase>();
                            Process(rule.ChildNodes,
                                cNodes,
                                parameterNames,
                                parameterValues);
                            rule.ChildNodes = cNodes;
                            outNodes.Add(rule);
                        }
                        break;
                    
                    case NodeToken token:
                        token.TokenValue = ReplaceParams(token.TokenValue, 
                            parameterNames,
                            parameterValues);
                        outNodes.Add(token);
                        break;
                    
                    case NodeComment comment:
                        comment.Comment = ReplaceParams(comment.Comment, 
                            parameterNames, 
                            parameterValues);
                        outNodes.Add(comment);
                        break;

                    default:
                        outNodes.Add(child);
                        break;
                }
                i += 1;
            }
            return true;
        }
    }
}
