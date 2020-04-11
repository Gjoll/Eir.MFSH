using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHer
{
    public abstract class NodeEntity: NodeBase
    {
        /// <summary>
        /// Name of entity.
        /// </summary>
        public String Name { get; set; }
    }
}
