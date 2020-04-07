using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp
{
    class NodeMappingRule : NodeBase
    {
        public String Contents;
        public NodeMappingRule() { this.NodeType = "mapping"; }
    }
}
