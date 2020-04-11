using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHer
{
    class NodeCardRule : NodeBase
    {
        public override string ToString() => $"CardRule";
        public NodeCardRule() { this.NodeType = "cardRule"; }
    }
}
