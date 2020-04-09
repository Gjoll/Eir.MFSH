using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp.Nodes
{
    class NodeEntityName : NodeBase
    {
        public String EntityName;
        public NodeEntityName() => this.NodeType = "entityName";
        public override string ToString() => $"EntityName: '{this.EntityName}'";
        public override string ToFSH() => this.EntityName;
    }
}
