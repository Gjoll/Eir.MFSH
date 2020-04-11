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
                switch (rule.NodeType.ToLower())
                {
                    case "alias":
                        this.FSHpp.AliasDict.Add(entityName, rule);
                        break;

                    case "profile":
                        this.FSHpp.ProfileDict.Add(entityName, rule);
                        break;

                    case "extension":
                        this.FSHpp.ExtensionDict.Add(entityName, rule);
                        break;

                    case "invariant":
                        this.FSHpp.InvariantDict.Add(entityName, rule);
                        break;

                    case "instance":
                        this.FSHpp.InstanceDict.Add(entityName, rule);
                        break;

                    case "valueset":
                        this.FSHpp.ValueSetDict.Add(entityName, rule);
                        break;

                    case "codesystem":
                        this.FSHpp.CodeSystemDict.Add(entityName, rule);
                        break;

                    case "ruleset":
                        this.FSHpp.RuleSetDict.Add(entityName, rule);
                        break;

                    case "mapping":
                        this.FSHpp.MappingDict.Add(entityName, rule);
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
        }

    }
}
