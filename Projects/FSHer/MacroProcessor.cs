using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHer
{
    public class MacroProcessor
    {
        private FSHer pp;

        /// <summary>
        /// Dictionary of rule sets that have had any macro's in them
        /// expended.
        /// </summary>
        Dictionary<String, NodeRule> ruleSetDict = new Dictionary<string, NodeRule>();

        public MacroProcessor(FSHer pp)
        {
            this.pp = pp;
        }


        public void Process()
        {
        }

    }
}
