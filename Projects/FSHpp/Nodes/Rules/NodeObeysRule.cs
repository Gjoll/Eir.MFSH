using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp
{
    class NodeObeysRule : NodeBase
    {
        public String Contents;
        public NodeObeysRule() { this.NodeType = "obeys"; }
    }
}
