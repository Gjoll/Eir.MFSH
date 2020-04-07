using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp
{
    class NodeValueSetRule : NodeBase
    {
        public String Contents;
        public NodeValueSetRule() { this.NodeType = "valueSet"; }
    }
}
