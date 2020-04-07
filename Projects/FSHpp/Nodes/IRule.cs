using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp
{
    public interface IRule
    {
        List<NodeBase> Rules{ get; }
    }
}
