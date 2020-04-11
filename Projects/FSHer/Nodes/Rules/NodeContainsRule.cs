using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHer
{
    class NodeContainsRule : NodeBase
    {
        public override string ToString() => $"ContainsRule";
        public NodeContainsRule() { this.NodeType = "containsRule"; }
    }
}
