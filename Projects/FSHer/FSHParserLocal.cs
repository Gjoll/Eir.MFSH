using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using Eir.FSHer.Antlr;

namespace Eir.FSHer
{
    public class FSHParserLocal : FSHParser
    {
        public bool DebugFlag { get; set; } = false;
        public FSHParserLocal(ITokenStream input) : base(input)
        {
        }

        public FSHParserLocal(ITokenStream input, TextWriter output, TextWriter errorOutput)
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
                    String tokenName = FSHListener.GetTokenName(localctx.RuleIndex);
                    System.Diagnostics.Trace.WriteLine($"Entering '{tokenName}'");
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
                    String tokenName = FSHListener.GetTokenName(localctx.RuleIndex);
                    System.Diagnostics.Trace.WriteLine($"Exiting'{tokenName}'");
                }
            }
        }
    }
}
