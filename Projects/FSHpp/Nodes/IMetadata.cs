using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp
{
    public interface IMetadata
    {
        List<NodeBase> SdMetadata { get; }
    }
}
