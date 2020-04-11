using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHer
{
    class NodeXPath : NodeBase
    {
        public String XPath { get; set; }
        public NodeXPath() { this.NodeType = "xpath"; }
    }
}
