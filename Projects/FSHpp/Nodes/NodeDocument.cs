using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp
{
    public class NodeDocument : NodeContainer
    {
        public String FileName;
        public String TrailingText;

        public override string ToFSH()
        {
            return base.ToFSH() + this.TrailingText;
        }
    }
}
