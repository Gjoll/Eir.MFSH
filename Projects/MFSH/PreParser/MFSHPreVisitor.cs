using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Dfa;
using Antlr4.Runtime.Tree;
using MFSH;
using Microsoft.VisualBasic.CompilerServices;

namespace MFSH.PreParser
{
    class MFSHPreVisitor : MFSHPreParserBaseVisitor<Object>
    {
        public bool DebugFlag { get; set; } = false;

        public string SourceName;

        public StringBuilder ParsedText = new StringBuilder();

        public MFSHPreVisitor(string sourceName)
        {
            this.SourceName = sourceName;
        }

        void TraceMsg(ParserRuleContext context, String fcn)
        {
            if (!DebugFlag)
                return;
            String text = context.GetText().Replace("\r", "").Replace("\n", "");
            Trace.WriteLine($"{fcn}. {this.SourceName}, #{context.Start.Line} '{text}'");
        }

        public override object VisitMfsh(MFSHPreParser.MfshContext context)
        {
            String text = context.GetText();
            Int32 lbIndex = text.IndexOf('#');
            text = text.Substring(0, lbIndex) + text.Substring(lbIndex + 1);
            Debug.Assert(text[^1] == '\n');
            this.ParsedText.Append(text);
            return null;
        }

        public override object VisitFsh(MFSHPreParser.FshContext context)
        {
            String text = context.GetText();
            Debug.Assert(text[^1] == '\n');
            text = text
                .Substring(0, text.Length-1)
                .Replace("\"", "\\\"")
                ;
            this.ParsedText.Append($"FshLine \"{text}\"\n");
            return null;
        }
    }
}
