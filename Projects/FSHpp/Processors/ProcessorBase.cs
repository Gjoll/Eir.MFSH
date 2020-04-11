using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp.Processors
{
    abstract class ProcessorBase
    {
        public FSHpp FSHpp;

        public ProcessorBase(FSHpp fshPp)
        {
            this.FSHpp = fshPp;
        }

        public abstract void Process();
    }
}
