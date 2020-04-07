using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp
{
    class NodeFlagRule : NodeBase
    {
        public String Contents;
        public NodeFlagRule() { this.NodeType = "flagRule"; }
    }
}
