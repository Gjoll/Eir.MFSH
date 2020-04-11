using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHer
{
    class NodeValueSetRule : NodeBase
    {
        public override string ToString() => $"ValueSetRule";
        public NodeValueSetRule() { this.NodeType = "valueSet"; }
    }
}
