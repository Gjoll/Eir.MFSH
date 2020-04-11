using System;
using System.Collections.Generic;
using System.Linq;
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
            CollateFile(f.Doc);
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
