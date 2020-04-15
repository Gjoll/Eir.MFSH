using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Eir.FSHer
{
    [DebuggerDisplay("'{TokenName}' {TokenValue}")]
    public class NodeToken : NodeBase
    {
        public String TokenName;
        public String TokenValue;

        public NodeToken()
        {
        }

        public NodeToken(String tokenName, String tokenValue) : base()
        {
            this.TokenName = tokenName;
            this.TokenValue = tokenValue;
        }

        public override string ToString() => Dump("");
        public override string Dump(String margin) => $"{margin}{TokenName}: '{this.TokenValue}'";
        public override string ToFSH() => this.TokenValue;

        public override void CopyTo(NodeBase itemBase)
        {
            NodeToken item = (NodeToken) itemBase;
            base.CopyTo(item);
            item.TokenName = this.TokenName;
            item.TokenValue = this.TokenValue;
        }

        public override NodeBase Clone()
        {
            NodeToken retVal = new NodeToken();
            this.CopyTo(retVal);
            return retVal;
        }
    }
}
