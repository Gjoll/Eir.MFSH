using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHer
{
    class NodeExpression : NodeBase
    {
        public String Expression { get; set; }
        public NodeExpression() { this.NodeType = "expression"; }
    }
}
