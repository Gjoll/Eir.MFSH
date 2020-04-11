using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace FSHer.Nodes
{
    class NodeSequence : NodeBase
    {
        public String Text;
        public NodeSequence() => this.NodeType = "sequence";
        public override string ToString() => $"NodeSequence: '{this.Text}'";
        public override string ToFSH() => this.Text;
    }
}
