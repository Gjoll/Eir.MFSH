using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHer
{
    public class FSHFile
    {
        public Dictionary<string, NodeRule> AliasDict = new Dictionary<string, NodeRule>();

        public String RelativePath;
        public NodeRule Doc;
    }
}
