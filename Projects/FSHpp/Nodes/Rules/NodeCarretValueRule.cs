using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp
{
    class NodeCaretValueRule : NodeBase
    {
        public override string ToString() => $"CaretValueRule";
        public NodeCaretValueRule() { this.NodeType = "caretValue"; }
    }
}
