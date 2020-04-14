using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FSHer.Processors
{
    /// <summary>
    /// Collates the various fsh entities, storing a reference to each
    /// one in its own dictionary.
    /// </summary>
    class Collator : ProcessorBase
    {
        FSHFile fshFile;

        public Collator(FSHer fsher) : base(fsher)
        {
        }


        public override void Process()
        {
            foreach (FSHFile f in this.FSHer.fshFiles)
                Process(f);
        }

        void Process(FSHFile f)
        {
            this.fshFile = f;
            CleanFile(f.Doc);
            CollateFile(f.Doc);
        }

        void CleanFile(NodeRule d)
        {
            foreach (NodeRule child in d.ChildNodes.Rules())
                CleanFile(child);


            List<NodeBase> childNodes = new List<NodeBase>();

            void CommaTokens(String tokenName, String tokenValue)
            {
                tokenValue = tokenValue.Trim();
                if (String.IsNullOrEmpty(tokenValue) == true)
                    return;
                String[] tokenValues = tokenValue.Trim().Split(',');

                childNodes.Add(new NodeToken(tokenName, tokenValues[0].Trim()));
                foreach (String v in tokenValues.Skip(1))
                {
                    childNodes.Add(new NodeComment(", "));
                    childNodes.Add(new NodeToken(tokenName, v.Trim()));
                }
            }

            foreach (NodeBase node in d.ChildNodes)
            {
                switch (node)
                {
                    case NodeToken token:
                        ///Convert strings that are concatanations of comma values into
                        /// seperate comma values.
                        switch (token.TokenName)
                        {
                            case "COMMA_DELIMITED_SEQUENCES":
                                CommaTokens("SEQUENCE", token.TokenValue);
                                break;

                            case "COMMA_DELIMITED_CODE":
                                CommaTokens("CODE", token.TokenValue);
                                break;

                            default:
                                childNodes.Add(node);
                                break;
                        }
                        break;

                    default:
                        childNodes.Add(node);
                        break;
                }

                d.ChildNodes = childNodes;
            }
        }
        void CollateFile(NodeRule d)
        {
            List<NodeBase> childNodes = new List<NodeBase>();

            foreach (NodeRule entity in d.ChildNodes.Rules())
            {
                if (entity.ChildNodes.Count != 1)
                    throw new Exception($"Invalid child nodes in entity record");
                NodeRule rule = (NodeRule)entity.ChildNodes.First();
                String entityName = rule.Name;
                switch (rule.RuleName)
                {
                    case "Alias":
                        this.fshFile.AliasDict.Add(entityName, rule);
                        childNodes.Add(rule);
                        break;

                    case "Profile":
                        this.FSHer.ProfileDict.Add(entityName, rule);
                        childNodes.Add(rule);
                        break;

                    case "Extension":
                        this.FSHer.ExtensionDict.Add(entityName, rule);
                        childNodes.Add(rule);
                        break;

                    case "Invariant":
                        this.FSHer.InvariantDict.Add(entityName, rule);
                        childNodes.Add(rule);
                        break;

                    case "Instance":
                        this.FSHer.InstanceDict.Add(entityName, rule);
                        childNodes.Add(rule);
                        break;

                    case "ValueSet":
                        this.FSHer.ValueSetDict.Add(entityName, rule);
                        break;

                    case "CodeSystem":
                        this.FSHer.CodeSystemDict.Add(entityName, rule);
                        childNodes.Add(rule);
                        break;

                    case "RuleSet":
                        this.FSHer.RuleSetDict.Add(entityName, rule);
                        childNodes.Add(rule);
                        break;

                    case "MacroDef":
                        this.FSHer.MacroDict.Add(entityName, rule);
                        break;

                    case "Mapping":
                        this.FSHer.MappingDict.Add(entityName, rule);
                        childNodes.Add(rule);
                        break;

                    default:
                        throw new NotImplementedException();
                }

                d.ChildNodes = childNodes;
            }
        }

    }
}
