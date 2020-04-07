using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp
{
    class NodeExpression : NodeBase
    {
        public String Expression { get; set; }
        public NodeExpression() { this.NodeType = "expression"; }
    }
}
