using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using MFSH;

namespace MFSH.Parser2
{
    public class MFSHParserLocal : MFSHParser
    {
        public bool DebugFlag { get; set; } = false;
        private TokenLookup tokens = new TokenLookup(Path.Combine("Parser", "ANTLR", "MFSHParser.tokens"));
        public MFSHParserLocal(ITokenStream input) : base(input)
        {
        }

        public MFSHParserLocal(ITokenStream input, TextWriter output, TextWriter errorOutput)
            : base(input, output, errorOutput)
        {
        }

        Stack<ParserRuleContext> contextStack = new Stack<ParserRuleContext>();
        public override void EnterRule(ParserRuleContext localctx, int state, int ruleIndex)
        {
            if (this.DebugFlag)
            {
                if (ruleIndex > 0)
                {
                    string ruleName = this.tokens.Lookup(localctx.RuleIndex);
                    System.Diagnostics.Trace.WriteLine($"Entering '{ruleName}'");
                }

                contextStack.Push(localctx);
            }

            base.EnterRule(localctx, state, ruleIndex);
        }

        public override void ExitRule()
        {
            base.ExitRule();

            if (this.DebugFlag)
            {
                ParserRuleContext localctx = contextStack.Pop();
                if (localctx.RuleIndex > 0)
                {
                    string ruleName = this.tokens.Lookup(localctx.RuleIndex);
                    System.Diagnostics.Trace.WriteLine($"Exiting'{ruleName}'");
                }
            }
        }
    }
}
