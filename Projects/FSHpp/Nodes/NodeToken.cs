using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp.Nodes
{
    class NodeToken : NodeBase
    {
        public String Token;
        public NodeToken() => this.NodeType = "token";
        public override string ToString() => $"Token: '{this.Token}'";
        public override string ToFSH() => this.Token;
    }
}
