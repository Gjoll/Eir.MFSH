using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp
{
    class NodeMixin : NodeBase
    {
        public String Name;
        public NodeMixin() { this.NodeType = "mixin"; }
    }
}
