using System;
using System.Collections.Generic;
using System.Text;
using MFSH;

namespace MFSH.Parser
{
    public class UseBlock : ParseBlock
    {
        public String Use { get; }

        public UseBlock(String sourceFile, String use) : base("Use")
        {
            this.Use = use;
        }
    }
}
