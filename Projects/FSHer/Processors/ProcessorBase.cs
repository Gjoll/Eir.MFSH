using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eir.FSHer.Processors
{
    abstract class ProcessorBase
    {
        public FSHer FSHer;

        public ProcessorBase(FSHer fsher)
        {
            this.FSHer = fsher;
        }

        public abstract void Process();
    }
}
