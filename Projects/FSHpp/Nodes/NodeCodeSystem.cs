using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp
{
    class NodeCodeSystem : NodeBase
    {
        public String Name;
        public NodeCodeSystem() { this.NodeType = "codeSystem"; }
    }
}
