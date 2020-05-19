using System;
using System.Collections.Generic;
using System.Text;
using Eir.MFSH;

namespace Eir.MFSH.Parser
{
    public class ParseBlock
    {
        public String OutputPath { get; set; }
        public List<MIBase> Items = new List<MIBase>();
        public ParseBlock()
        {
        }
    }
}
