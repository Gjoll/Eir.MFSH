using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHer
{
    class NodeFlagRule : NodeBase
    {
        public override string ToString() => $"FlagRule";
        public NodeFlagRule() { this.NodeType = "flagRule"; }
    }
}
