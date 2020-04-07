using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp
{
    public class NodeDocument: NodeBase
    {
        public String FileName;
        public String TrailingText;

        public override string ToFSH()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToFSH());
            foreach (NodeBase n in ChildNodes)
                sb.Append(n.ToFSH());
            sb.Append(this.TrailingText);
            return sb.ToString();
        }
    }
}
