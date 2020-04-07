using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp
{
    class NodeInstanceOf : NodeBase
    {
        public String InstanceOf { get; set; }
        public NodeInstanceOf() { this.NodeType = "instanceOf"; }
    }
}
