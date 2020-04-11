using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHer
{
    class NodeObeysRule : NodeBase
    {
        public override string ToString() => $"ObeysRule";
        public NodeObeysRule() { this.NodeType = "obeys"; }
    }
}
