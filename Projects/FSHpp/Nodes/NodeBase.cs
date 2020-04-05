using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp
{
    public abstract class NodeBase
    {
        public String Name;
        public String Comments;
        public abstract String ToFSH();
    }
}
