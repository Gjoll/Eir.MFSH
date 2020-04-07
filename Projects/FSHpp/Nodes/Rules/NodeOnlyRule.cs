using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp
{
    class NodeOnlyRule : NodeBase
    {
        public String Contents;
        public NodeOnlyRule() { this.NodeType = "only"; }
    }
}
