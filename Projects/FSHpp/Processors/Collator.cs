using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp.Processors
{
    /// <summary>
    /// Collates the various fsh entities, storing a reference to each
    /// one in its own dictionary.
    /// </summary>
    class Collator : ProcessorBase
    {
        public Collator(FSHpp fshPp) : base(fshPp)
        {
        }


        public override void Process()
        {
            foreach (FSHpp.FSHFile f in this.FSHpp.fshFiles)
                Process(f);
        }

        void Process(FSHpp.FSHFile f)
        {
            CollateFile(f.Doc);
        }

        void CollateFile(NodeRule d)
        {
            foreach (NodeRule entity in d.ChildNodes.Rules())
            {
                if (entity.ChildNodes.Count != 1)
                    throw new Exception($"Invalid child nodes in entity record");
                NodeRule rule = (NodeRule)entity.ChildNodes.First();
                String entityName = rule.Name;
                switch (rule.RuleName)
                {
                    case "Alias":
                        this.FSHpp.AliasDict.Add(entityName, rule);
                        break;

                    case "Profile":
                        this.FSHpp.ProfileDict.Add(entityName, rule);
                        break;

                    case "Extension":
                        this.FSHpp.ExtensionDict.Add(entityName, rule);
                        break;

                    case "Invariant":
                        this.FSHpp.InvariantDict.Add(entityName, rule);
                        break;

                    case "Instance":
                        this.FSHpp.InstanceDict.Add(entityName, rule);
                        break;

                    case "ValueSet":
                        this.FSHpp.ValueSetDict.Add(entityName, rule);
                        break;

                    case "CodeSystem":
                        this.FSHpp.CodeSystemDict.Add(entityName, rule);
                        break;

                    case "RuleSet":
                        this.FSHpp.RuleSetDict.Add(entityName, rule);
                        break;

                    case "MacroDef":
                        this.FSHpp.MacroDict.Add(entityName, rule);
                        break;

                    case "Mapping":
                        this.FSHpp.MappingDict.Add(entityName, rule);
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
        }

    }
}
