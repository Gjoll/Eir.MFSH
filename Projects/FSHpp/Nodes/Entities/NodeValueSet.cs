using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp
{
    class NodeValueSet : NodeBase
    {
        public String Name;
        public NodeValueSet() { this.NodeType = "valueSet"; }
    }
}
