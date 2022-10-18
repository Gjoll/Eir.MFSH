using System;
using System.Collections.Generic;
using System.Text;
using MFSH;

namespace MFSH.Parser
{
    public class ParseBlock
    {
        public List<MIBase> Items = new List<MIBase>();
        public String Name { get; }
        public ParseBlock(String name)
        {
            this.Name = name;
        }
    }
}
