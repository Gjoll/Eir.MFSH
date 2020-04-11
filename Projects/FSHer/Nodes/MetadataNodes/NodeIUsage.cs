using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHer
{
    class NodeUsage : NodeBase
    {
        public String Usage { get; set; }
        public NodeUsage() { this.NodeType = "usage"; }
    }
}
