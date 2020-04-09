using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp.Nodes
{
    class NodeComment : NodeBase
    {
        public String Comment;
        public NodeComment() => this.NodeType = "comment";
        public override string ToString() => $"Comment: '{this.Comment.Replace("\n", "\\n")}'";
        public override string ToFSH() => this.Comment;
    }
}
