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
        public String NodeType;
        public String Comments;
        public String Code;

        public NodeBase() : base()
        {
        }

        public virtual String ToFSH()
        {
            return this.Comments + this.Code;
        }
    }
}
