using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp
{
    public class MacroProcessor
    {
        private FSHpp pp;

        /// <summary>
        /// Dictionary of rule sets that have had any macro's in them
        /// expended.
        /// </summary>
        Dictionary<String, NodeRule> ruleSetDict = new Dictionary<string, NodeRule>();

        public MacroProcessor(FSHpp pp)
        {
            this.pp = pp;
        }


        public void Process()
        {
        }

    }
}
