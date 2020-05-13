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
            TraceMsg(context, "Mfsh");
            String text = context.GetText();
            Int32 lbIndex = text.IndexOf('#');
            text = text.Substring(lbIndex + 1);
            Debug.Assert(text[^1] == '\n');
            this.ParsedText.Append(text);
            return null;
        }

        void OutputFshLine(String text)
        {
            Debug.Assert(text[^1] == '\n');
            text = text
                    .Substring(0, text.Length - 1)
                    .Replace("\"", "\\\"")
                    .Replace("\n", "\\n")
                ;
            this.ParsedText.Append($"FshLine \"{text}\"\n");
        }

        public override object VisitFshCmd(MFSHPreParser.FshCmdContext context)
        {
            TraceMsg(context, "FshCmd");
            String text = context.GetText();
            OutputFshLine(text);

            String command = context.TEXT(0).GetText();
            switch (command)
            {
                case "Profile":
                    String profileName = context.TEXT(1).GetText();
                    this.ParsedText.Append($"profile {profileName}\n");
                    break;
            }
            return null;
        }

        public override object VisitTickData(MFSHPreParser.TickDataContext context)
        {
            TraceMsg(context, "TickData");
            String text = context.GetText();
            text = text.Substring(text.IndexOf('`') + 1);
            OutputFshLine(text);
            return null;
        }

        public override object VisitData(MFSHPreParser.DataContext context)
        {
            TraceMsg(context, "Data");
            String text = context.GetText();
            OutputFshLine(text);
            return null;
        }
    }
}
