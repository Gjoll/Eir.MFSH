using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp
{
    class NodeExtension : NodeBase, IMetadata, IRule
    {
        public String Name;
        public List<NodeBase> Metadata { get; } = new List<NodeBase>();
        public List<NodeBase> Rules { get; } = new List<NodeBase>();
        public NodeExtension() { this.NodeType = "extension"; }
    }
}
