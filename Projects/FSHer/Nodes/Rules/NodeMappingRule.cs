using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHer
{
    class NodeMappingRule : NodeBase
    {
        public override string ToString() => $"MappingRule";
        public NodeMappingRule() { this.NodeType = "mapping"; }
    }
}
