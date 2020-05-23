using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Dfa;
using Antlr4.Runtime.Tree;
using Eir.MFSH;
using Microsoft.VisualBasic.CompilerServices;

namespace Eir.MFSH.Parser
{
    class MFSHVisitor : MFSHParserBaseVisitor<Object>
    {
        public bool DebugFlag { get; set; } = false;
        MFshManager mfshMgr;

        public Stack<ParseBlock> state = new Stack<ParseBlock>();
        public ParseBlock Current => this.state.Peek();
        public string SourceName;

        void PushState(ParseBlock s)
        {
            this.state.Push(s);
        }

        ParseBlock PopState()
        {
            ParseBlock s = this.state.Peek();
            this.state.Pop();
            return s;
        }

        public MFSHVisitor(MFshManager mfshManager,
            string sourceName)
        {
            this.SourceName = sourceName;
            this.mfshMgr = mfshManager;
            ParseBlock f = new ParseBlock();
            this.PushState(f);
        }

        void TraceMsg(ParserRuleContext context, String fcn)
        {
            if (!DebugFlag)
                return;
            String text = context
                .GetText()
                .Replace("\r", "")
                .Replace("\n", "");
            Trace.WriteLine($"{fcn}. {this.SourceName}, #{context.Start.Line} '{text}'");
        }

        public override object VisitDocument(MFSHParser.DocumentContext context)
        {
            const String fcn = "VisitDocument";
            TraceMsg(context, fcn);
            this.VisitChildren(context);
            return null;
        }

        public override object VisitTextA(MFSHParser.TextAContext context)
        {
            const String fcn = "VisitTextA";
            TraceMsg(context, fcn);

            String line = context.GetText();
            MIText t = new MIText(this.SourceName, context.Start.Line)
            {
                Line = context.GetText()
            };
            this.Current.Items.Add(t);
            return null;
        }

        public override object VisitTextB(MFSHParser.TextBContext context)
        {
            const String fcn = "VisitTextB";
            TraceMsg(context, fcn);

            String line = context.GetText();
            MIText t = new MIText(this.SourceName, context.Start.Line)
            {
                Line = context.GetText()
            };
            this.Current.Items.Add(t);
            return null;
        }

        public override object VisitTextC(MFSHParser.TextCContext context)
        {
            const String fcn = "VisitTextC";
            TraceMsg(context, fcn);

            String line = context.GetText();
            MIText t = new MIText(this.SourceName, context.Start.Line)
            {
                Line = context.GetText()
            };
            this.Current.Items.Add(t);
            return null;
        }

        public override object VisitTextD(MFSHParser.TextDContext context)
        {
            const String fcn = "VisitTextD";
            TraceMsg(context, fcn);

            String line = context.GetText();
            MIText t = new MIText(this.SourceName, context.Start.Line)
            {
                Line = context.GetText()
            };
            this.Current.Items.Add(t);
            return null;
        }

        public override object VisitTickText(MFSHParser.TickTextContext context)
        {
            const String fcn = "VisitTickText";
            TraceMsg(context, fcn);

            String line = context.GetText();
            Int32 tickIndex = line.IndexOf('`');
            line = line.Substring(tickIndex + 1);
            MIText t = new MIText(this.SourceName, context.Start.Line)
            {
                Line = line
            };
            this.Current.Items.Add(t);
            return null;
        }

        public override object VisitMacro(MFSHParser.MacroContext context)
        {
            const String fcn = "VisitMacro";
            TraceMsg(context, fcn);
            MacroBlock macroBlock = new MacroBlock(this.SourceName, context.Start.Line);
            this.PushState(macroBlock);
            String[] names = context
                .NAME()
                .Select((a) => a.GetText())
                .ToArray();
            macroBlock.Macro.Name = names[0];
            macroBlock.Macro.Parameters.AddRange(names.Skip(1));
            var redirectContext = context.redirect();
            if (redirectContext != null)
                macroBlock.Macro.Redirect = (String)(this.Visit(redirectContext.singleString()));

            macroBlock.Macro.OnceFlag = (context.ONCE() != null);
            if (
                (macroBlock.Macro.OnceFlag == true) &&
                (macroBlock.Macro.Parameters.Count > 0))
            {
                this.Error(fcn,
                    context.Start.Line.ToString(),
                    $"Error adding macro '{macroBlock.Macro.Name}'. OnceFlag can not be used with macro parameters");
                return null;
            }

            return null;
        }

        public override object VisitIncompatible(MFSHParser.IncompatibleContext context)
        {
            //const String fcn = "VisitIncompatible";
            String macroName = context.NAME().GetText();

            MIIncompatible incompatible = new MIIncompatible(this.SourceName, context.Start.Line)
            {
                Name = macroName
            };

            this.Current.Items.Add(incompatible);
            return null;
        }

        public override object VisitApply(MFSHParser.ApplyContext context)
        {
            //const String fcn = "VisitApply";
            MIApply apply = new MIApply(this.SourceName, context.Start.Line);
            apply.Name = context.NAME().GetText();

            foreach (MFSHParser.AnyStringContext varContext in context.anyString())
            {
                String s = (String)this.VisitChildren(varContext);
                apply.Parameters.Add(s);
            }

            this.Current.Items.Add(apply);
            return null;
        }


        public override object VisitEnd(MFSHParser.EndContext context)
        {
            const String fcn = "VisitEnd";
            TraceMsg(context, fcn);

            ParseBlock s = this.PopState();
            switch (s)
            {
                case MacroBlock macroBlock:

                    if (this.mfshMgr.TryAddMacro(macroBlock.Macro.Name, macroBlock.Macro) == false)
                    {
                        this.Error(fcn,
                            context.Start.Line.ToString(),
                            $"Error adding macro '{macroBlock.Macro.Name}'. Does macro already exist?");
                    }
                    break;

                default:
                    Error(fcn,
                        context.Start.Line.ToString(),
                        $"Unexpected '#end'");
                    throw new Exception($"Unexpected '#end'");
            }
            return null;
        }

        public override object VisitAnyString(MFSHParser.AnyStringContext context)
        {
            return (String)this.VisitChildren(context);
        }

        public override object VisitSingleString(MFSHParser.SingleStringContext context)
        {
            String s = context.GetText();
            s = s
                .Substring(1, s.Length - 2)
                .Replace("\\\"", "\"")
                .Replace("\\\\", "\\")
                ;
            return s;
        }

        public override object VisitMultiLineString(MFSHParser.MultiLineStringContext context)
        {
            String text = context.GetText().Trim();
            List<String> lines = context.GetText().Split('\n').ToList();
            if (lines.Count == 0)
                return null;

            for (Int32 i = 0; i < lines.Count; i++)
            {
                String line = lines[i];
                {
                    Regex r = new Regex("^[ ]*#");
                    Match m = r.Match(line);
                    if (m.Success == true)
                        line = line.Substring(m.Length);
                }
                lines[i] = line;
            }

            if (lines[0].StartsWith("\"\"\""))
                lines[0] = lines[0].Substring(3);
            if (lines[^1].EndsWith("\"\"\""))
                lines[^1] = lines[^1].Substring(0, lines[^1].Length - 3);
            while (String.IsNullOrWhiteSpace(lines[0]))
                lines.RemoveAt(0);
            while (String.IsNullOrWhiteSpace(lines[^1]))
                lines.RemoveAt(lines.Count - 1);

            Int32 minMargin = Int32.MaxValue;
            for (Int32 i = 0; i < lines.Count; i++)
            {
                String line = lines[i];
                Regex r = new Regex("^[ ]*");
                Match m = r.Match(line);
                if ((m.Success == true) && (m.Length < minMargin))
                    minMargin = m.Length;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(lines[0].Substring(minMargin));
            foreach (String line in lines.Skip(1))
            {
                sb.Append("\n");
                sb.Append(line.Substring(minMargin));
            }
            return sb.ToString();
        }

        void Error(String fcn,
            String location,
            String msg)
        {
            String fullMsg = $"{this.SourceName}, line {location}. {msg}";
            this.mfshMgr.Mfsh.ConversionError("mfsh", fcn, fullMsg);
        }
    }
}
