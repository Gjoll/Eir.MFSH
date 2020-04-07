using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp
{
    class NodeTarget : NodeBase
    {
        public String Target { get; set; }
        public NodeTarget() { this.NodeType = "target"; }
    }
}
