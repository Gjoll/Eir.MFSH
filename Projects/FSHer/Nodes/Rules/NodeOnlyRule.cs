using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHer
{
    class NodeOnlyRule : NodeBase
    {
        public override string ToString() => $"OnlyRule";
        public NodeOnlyRule() { this.NodeType = "only"; }
    }
}
