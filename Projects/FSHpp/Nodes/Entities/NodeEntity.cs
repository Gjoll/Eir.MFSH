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
        /// Name of entity.
        /// </summary>
        public String Name { get; set; }
    }
}
