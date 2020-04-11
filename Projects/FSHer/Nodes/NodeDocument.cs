using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHer
{
    public class NodeDocument: NodeBase
    {
        /// <summary>
        /// Not read from input text.
        /// </summary>
        public String FileName;

        public NodeDocument() => this.NodeType = "document";
    }
}
