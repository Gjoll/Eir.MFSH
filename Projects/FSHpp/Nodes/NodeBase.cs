using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp
{
    public abstract class NodeBase
    {
        public String NodeType { get; set; }

        public NodeBase(String nodeType)
        {
            this.NodeType = nodeType;
        }

        public abstract String ToFSH();
        public abstract String Dump(String margin);
    }
}
