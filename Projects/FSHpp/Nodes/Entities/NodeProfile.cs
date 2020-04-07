using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSHpp;

namespace FSHpp
{
    public class NodeProfile : NodeBase, IMetadata, IRule
    {
        public String Name;
        public List<NodeBase> Metadata { get; } = new List<NodeBase>();
        public List<NodeBase> Rules{ get; } = new List<NodeBase>();
        public NodeProfile() { this.NodeType = "profile"; }
    }
}
