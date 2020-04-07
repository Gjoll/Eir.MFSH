using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp
{
    class NodeDescription : NodeBase
    {
        public String Description { get; set; }
        public NodeDescription() { this.NodeType = "description"; }
    }
}
