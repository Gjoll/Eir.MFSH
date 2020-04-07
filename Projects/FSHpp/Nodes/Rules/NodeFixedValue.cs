using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp
{
    class NodeFixedValueRule : NodeBase
    {
        public String Contents;
        public NodeFixedValueRule() { this.NodeType = "fixedValue"; }
    }
}
