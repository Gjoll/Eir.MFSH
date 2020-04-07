using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp
{
    class NodeRuleSet : NodeBase, IRule
    {
        public String Name;
        public List<NodeBase> Rules { get; } = new List<NodeBase>();
        public NodeRuleSet() { this.NodeType = "ruleSet"; }
    }
}
