using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHer
{
    class NodeFixedValueRule : NodeBase
    {
        public override string ToString() => $"FixedValueRule";
        public NodeFixedValueRule() { this.NodeType = "fixedValue"; }
    }
}
