using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp
{
    public class NodeContainer : NodeBase, IContainer
    {
        public List<NodeBase> Nodes { get; }  = new List<NodeBase>();

        public override string ToFSH()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToFSH());
            foreach (NodeBase n in Nodes)
                sb.Append(n.ToFSH());
            return sb.ToString();
        }
    }
}
