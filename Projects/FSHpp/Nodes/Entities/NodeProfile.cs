using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSHpp;

namespace FSHpp
{
    public class NodeProfile : NodeBase
    {
        public String Name;
        public NodeProfile() { this.NodeType = "profile"; }
    }
}
