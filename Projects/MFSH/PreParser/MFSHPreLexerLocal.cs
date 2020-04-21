using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using MFSH;

namespace MFSH.PreParser
{
    public class MFSHPreLexerLocal : MFSHPreLexer
    {
        public bool DebugFlag { get; set; } = false;
        private TokenLookup tokens = new TokenLookup(Path.Combine("PreParser", "ANTLR", "MFSHPreLexer.tokens"));

        public MFSHPreLexerLocal(ICharStream input) : base(input)
        {
        }

        public MFSHPreLexerLocal(ICharStream input, TextWriter output, TextWriter errorOutput)
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
