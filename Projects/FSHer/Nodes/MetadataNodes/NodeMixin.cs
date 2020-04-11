using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHer
{
    class NodeMixin : NodeBase
    {
        public NodeMixin() { this.NodeType = "mixin"; }
        public override string ToString() => $"Mixin'";
    }
}
