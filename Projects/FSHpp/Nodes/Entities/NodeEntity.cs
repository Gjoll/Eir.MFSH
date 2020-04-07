using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp
{
    public abstract class NodeEntity: NodeBase
    {
        /// <summary>
        /// All text up to name, including spaces and line breaks.
        /// </summary>
        public String Header { get; set; }

        /// <summary>
        /// Name of entity.
        /// </summary>
        public String Name { get; set; }

        public override string ToFSH()
        {
            return Header + Name;
        }
    }
}
