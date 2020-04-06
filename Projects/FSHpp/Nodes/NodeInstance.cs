using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp
{
    class NodeInstance : NodeBase
    {
        public String Name;
        public NodeInstance() { this.NodeType = "instance"; }
    }
}
