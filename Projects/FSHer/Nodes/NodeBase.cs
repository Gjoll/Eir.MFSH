using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Eir.FSHer
{
    public abstract class NodeBase
    {
        public abstract String ToFSH();
        public abstract String Dump(String margin);

        public abstract NodeBase Clone();

        public virtual void CopyTo(NodeBase item)
        {
        }
    }
}
