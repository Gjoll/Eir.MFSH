using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHer
{
    class NodeParent : NodeBase
    {
        public String Name;
        public NodeParent() { this.NodeType = "parent"; }
    }
}
