using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp
{
    [DebuggerDisplay("{NodeType}: '{TokenName}' {TokenValue}")]
    public class NodeToken : NodeBase
    {
        public String TokenName;
        public String TokenValue;

        public NodeToken(String nodeType) : base(nodeType)
        {
        }

        public override string ToString() => Dump("");
        public override string Dump(String margin) => $"{margin}{TokenName}: '{this.TokenValue}'";
        public override string ToFSH() => this.TokenValue;
    }
}
