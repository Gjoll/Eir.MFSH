using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Eir.FSHer
{
    [DebuggerDisplay("{RuleName} {ChildNodes.Count}")]
    public class NodeRule : NodeBase
    {
        public List<NodeBase> ChildNodes { get; set; } = new List<NodeBase>();
        public String RuleName { get; set;  }

        public NodeRule()
        {
        }

        public NodeRule(String ruleName) : base()
        {
            this.RuleName = ruleName;
        }

        public String Name => this.ChildNodes
                .Tokens()
                .WithTokenName("SEQUENCE")
                .First()
                .TokenValue
            ;

        public List<String> Strings => this.ChildNodes
            .Tokens()
            .WithTokenName("STRING")
            .Select(s => s.TokenValue)
            .RemoveQuotes()
            .ToList()
            ;

        public override string ToString() => Dump("");
        public override string Dump(String margin)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{margin}{this.RuleName}");
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

        public override NodeBase Clone()
        {
            NodeRule retVal = new NodeRule();
            this.CopyTo(retVal);
            return retVal;
        }

        public override void CopyTo(NodeBase itemBase)
        {
            NodeRule item = (NodeRule) itemBase;
            base.CopyTo(item);
            item.RuleName = this.RuleName;
            item.ChildNodes.Clear();
            foreach (NodeBase childNode in this.ChildNodes)
                item.ChildNodes.Add(childNode.Clone());
        }
    }
}
