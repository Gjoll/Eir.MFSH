using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp
{
    class NodeInvariant: NodeBase
    {
        public String Name;
        public NodeInvariant() { this.NodeType = "invariant"; }
    }
}
