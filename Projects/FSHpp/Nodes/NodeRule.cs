using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp
{
    [DebuggerDisplay("{NodeType} {ChildNodes.Count}")]
    public class NodeRule : NodeBase
    {
        public List<NodeBase> ChildNodes { get; set; } = new List<NodeBase>();

        public NodeRule(String nodeType) : base(nodeType)
        {
        }

        public String Name => this.ChildNodes
                .Tokens()
                .WithTokenName("SEQUENCE")
                .First()
                .TokenValue
            ;

        public override string ToString() => Dump("");
        public override string Dump(String margin)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{margin}{this.NodeType}");
            foreach (NodeBase child in ChildNodes)
            {
                sb.AppendLine(child.Dump(margin + "    "));
            }

            return sb.ToString();

        }

        public override String ToFSH()
        {
            StringBuilder sb = new StringBuilder();
            foreach (NodeBase child in ChildNodes)
                sb.Append(child.ToFSH());
            return sb.ToString();
        }
    }
}
