using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHer
{
    class NodeAlias : NodeEntity
    {
        public NodeAlias() {this.NodeType = "alias"; }
        public String Value;
    }
}
