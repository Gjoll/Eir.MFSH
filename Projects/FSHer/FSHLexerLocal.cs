using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using FSHer.Antlr;

namespace FSHer
{
    public class FSHLexerLocal : FSHLexer
    {

        public FSHLexerLocal(ICharStream input) : base(input)
        {
        }

        public FSHLexerLocal(ICharStream input, TextWriter output, TextWriter errorOutput)
            : base(input, output, errorOutput)
        {
        }

        public override IToken NextToken()
        {
            IToken retVal = base.NextToken();
            string tokenName;
            if (retVal.Type == -1)
                tokenName = "<eof>";
            else
                tokenName = FSHListener.GetTokenName(retVal.Type);

            Trace.WriteLine($"{tokenName}: '{retVal.Text}'");
            return retVal;
        }
    }
}
