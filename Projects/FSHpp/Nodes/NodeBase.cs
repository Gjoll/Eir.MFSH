using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp
{

    [DebuggerDisplay("{ToFSH(false).ToString()}")]
    public class NodeBase
    {
        public String NodeType;

        public List<NodeBase> ChildNodes { get; } = new List<NodeBase>();

        public NodeBase() : base()
        {
        }

        public virtual String ToFSH()
        {
            StringBuilder sb = new StringBuilder();
            foreach (NodeBase child in ChildNodes)
                sb.Append(child.ToFSH());
            return sb.ToString();
        }
    }
}
