using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using MFSH.Antlr;

namespace MFSH
{
    public class MFSHLexerLocal : MFSHLexer
    {
        public bool DebugFlag { get; set; } = false;
        private TokenLookup tokens = new TokenLookup("MFSHLexer.tokens");

        public MFSHLexerLocal(ICharStream input) : base(input)
        {
        }

        public MFSHLexerLocal(ICharStream input, TextWriter output, TextWriter errorOutput)
            : base(input, output, errorOutput)
        {
        }

        public override IToken NextToken()
        {
            IToken retVal = base.NextToken();
            if (this.DebugFlag)
            {
                string tokenName = this.tokens.Lookup(retVal.Type);
                String tokenText = retVal.Text.Replace("\n", "\\n").Replace("\r", "\\r");
                Trace.WriteLine($"{tokenName}: '{tokenText}'");
            }

            return retVal;
        }
    }
}
