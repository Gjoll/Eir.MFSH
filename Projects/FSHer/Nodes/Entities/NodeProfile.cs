using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSHer;

namespace FSHer
{
    public class NodeProfile : NodeEntity
    {
        public NodeProfile() { this.NodeType = "profile"; }

        public override string ToFSH()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Profile: {this.Name}");
            foreach (NodeBase n in ChildNodes)
                sb.Append(n.ToFSH());
            return sb.ToString();
        }

    }
}
