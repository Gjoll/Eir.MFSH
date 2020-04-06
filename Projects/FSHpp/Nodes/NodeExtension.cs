using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp
{
    class NodeExtension : NodeBase, IMetadata
    {
        public String Name;
        public List<NodeBase> SdMetadata { get; } = new List<NodeBase>();
        public NodeExtension() { this.NodeType = "extension"; }
    }
}
