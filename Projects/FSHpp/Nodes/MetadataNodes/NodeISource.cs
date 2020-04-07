using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp
{
    class NodeSource : NodeBase
    {
        public String Source { get; set; }
        public NodeSource() { this.NodeType = "source"; }
    }
}
