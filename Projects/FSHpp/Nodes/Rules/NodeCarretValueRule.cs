using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp
{
    class NodeCaretValueRule : NodeBase
    {
        public String Contents;
        public NodeCaretValueRule() { this.NodeType = "caretValue"; }
    }
}
