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
using MFSH;
using Microsoft.VisualBasic.CompilerServices;

namespace MFSH.Parser2
{
    class MFSHVisitor : MFSHParserBaseVisitor<Object>
    {
        public bool DebugFlag { get; set; } = false;

        public Stack<ParseBlock> state = new Stack<ParseBlock>();
        public ParseBlock Current => this.state.Peek();
        public string SourceName;
        MFsh mfsh;

        protected void AppendText(String text)
        {
            this.Current.Data.AppendText(text);
        }
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

        public MFSHVisitor(MFsh mfsh,
            string sourceName)
        {
            this.SourceName = sourceName;
            this.mfsh = mfsh;
            ParseBlock f = new ParseBlock();
            f.Data.RelativePath = "%SavePath%";
            this.PushState(f);
        }

        void TraceMsg(ParserRuleContext context, String fcn)
        {
            //if (!DebugFlag)
            //    return;
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

        public override object VisitMacro(MFSHParser.MacroContext context)
        {
            const String fcn = "VisitMacro";
            //TraceMsg(context, fcn);
            //MacroDefinition macroDefinition = new MacroDefinition();
            //this.PushState(macroDefinition);
            //String[] names = context
            //    .NAME()
            //    .Select((a) => a.GetText())
            //    .ToArray();
            //macroDefinition.Name = names[0];
            //macroDefinition.Parameters.AddRange(names.Skip(1));

            //if (this.Current.IncompatibleMacros.Contains(macroDefinition.Name))
            //{
            //    this.Error(fcn,
            //        context.Start.Line.ToString(),
            //        $"Macro {macroDefinition.Name} has been marked as incompatible");
            //    return false;
            //}

            //var redirectContext = context.redirect();
            //if (redirectContext != null)
            //    macroDefinition.Data.RelativePath = (String)(this.Visit(redirectContext.singleString()));
            return null;
        }

        bool LoadApplyParams(VariablesBlock variablesBlock,
            IEnumerable<MFSHParser.AnyStringContext> varsContext,
            MacroDefinition info,
            IToken start)
        {
            const String fcn = "LoadApplyParams";

            List<String> parameters = new List<string>();
            foreach (MFSHParser.AnyStringContext varContext in varsContext)
            {
                String s = (String)this.VisitChildren(varContext);
                parameters.Add(s);
            }

            if (info.Parameters.Count != parameters.Count)
            {
                this.Error(fcn,
                    start.Line.ToString(),
                    $"Macro {info.Name} requires {info.Parameters.Count} parameters, but only {parameters.Count} supplied.");
                return false;
            }

            for (Int32 i = 0; i < info.Parameters.Count; i++)
            {
                String key = info.Parameters[i];
                String value = parameters[i];
                variablesBlock.Set(key, value);
            }

            return true;
        }

        public override object VisitIncompatible(MFSHParser.IncompatibleContext context)
        {
            const String fcn = "VisitIncompatible";
            String macroName = context.NAME().GetText();

            //if (this.Current.IncompatibleMacros.Contains(macroName))
            //    return null;
            //this.Current.IncompatibleMacros.Add(macroName);
            //if (this.Current.AppliedMacros.ContainsKey(macroName))
            //{
            //    this.Error(fcn,
            //        context.Start.Line.ToString(),
            //        $"Incompatible macro {macroName} has already been applied");
            //    return false;
            //}

            return null;
        }

        public override object VisitApply(MFSHParser.ApplyContext context)
        {
            //const String fcn = "VisitApply";
            //MacroDefinition info;
            //String macroName = context.NAME().GetText();
            //string text;
            //VariablesBlock parameterValues = new VariablesBlock();
            //bool onceFlag = (context.ONCE() != null);

            //// Check to se if macro has already been applied.
            //// return true to apply it, false to not apply it.
            //bool CheckOnce()
            //{
            //    if (
            //        (onceFlag) &&
            //        (parameterValues.Count > 0)
            //        )
            //    {
            //        this.Error(fcn,
            //            context.Start.Line.ToString(),
            //            $"Attempt to apply macro {macroName} with once and variables (once does not work with parameterized macros)");
            //        return false;
            //    }

            //    if (CheckApplyMacro(macroName, onceFlag, context.Start.Line) == false)
            //        return false;

            //    foreach (ApplyInfo applyInfo in info.AppliedMacros.Values)
            //        CheckApplyMacro(applyInfo.MacroName, applyInfo.OnceFlag, context.Start.Line);

            //    return true;
            //}

            //TraceMsg(context, fcn);

            //if (this.mfsh.Defines.TryGetValue(macroName, out info) == false)
            //{
            //    this.Error(fcn,
            //        context.Start.Line.ToString(),
            //        $"Macro {macroName} not found.");
            //    return null;
            //}

            //// Copy each incompatible macro from applied macro to current. Check to see if any of the new
            //// incompatible entries match an already loaded macro (if so, generate error message)
            //foreach (String incompatibleMacro in info.IncompatibleMacros)
            //{
            //    if (this.Current.IncompatibleMacros.Contains(incompatibleMacro) == false)
            //    {
            //        if (this.Current.AppliedMacros.ContainsKey(incompatibleMacro))
            //        {
            //            this.Error(fcn,
            //                context.Start.Line.ToString(),
            //                $"Incompatible macro {incompatibleMacro} has already been applied");
            //            return false;
            //        }
            //        this.Current.IncompatibleMacros.Add(incompatibleMacro);
            //    }
            //}

            //foreach (ApplyInfo appliedMacro in info.AppliedMacros.Values)
            //{
            //    if (this.Current.AppliedMacros.Keys.Contains(appliedMacro.MacroName) == false)
            //    {
            //        if (this.Current.IncompatibleMacros.Contains(appliedMacro.MacroName))
            //        {
            //            this.Error(fcn,
            //                context.Start.Line.ToString(),
            //                $"Incompatible macro {appliedMacro.MacroName} has already been applied");
            //            return false;
            //        }
            //        this.Current.AppliedMacros.Add(appliedMacro.MacroName, appliedMacro);
            //    }
            //}

            //if (LoadApplyParams(parameterValues, context.anyString(), info, context.Start) == false)
            //    return null;
            //text = info.Data.Text();
            //text = parameterValues.ReplaceText(text);
            //text = this.Current.FrameVariables.ReplaceText(text);

            //// See if macro was applied previously with once flag.
            //if (CheckOnce() == false)
            //    return null;

            ///*
            // * Output of macro either goes to current output, or to a redirected target.
            // */
            //if (String.IsNullOrEmpty(info.Data.RelativePath) == false)
            //{
            //    String rPath = parameterValues.ReplaceText(info.Data.RelativePath);
            //    rPath = this.Current.FrameVariables.ReplaceText(rPath);
            //    FileData redirOut = new FileData
            //    {
            //        RelativePath = rPath,
            //        RelativePathType = info.Data.RelativePathType
            //    };
            //    this.Current.Redirections.Add(redirOut);
            //    redirOut.AppendText(text);
            //}
            //else
            //{
            //    if (onceFlag == true)
            //    {
            //        this.Current.Data.AppendText($"\n#apply once {macroName}\n");
            //        this.Current.Data.AppendText(text);
            //        this.Current.Data.AppendText($"\n#end\n");
            //    }
            //    else
            //    {
            //        this.Current.Data.AppendText(text);
            //    }
            //}

            //// Make a copy of redir and process variables. Don't
            //// change original because it may be used again.
            //foreach (FileData redir in info.Redirections)
            //{

            //    FileData fd = new FileData
            //    {
            //        RelativePath = redir.RelativePath,
            //        RelativePathType = redir.RelativePathType
            //    };
            //    fd.AppendText(redir.Text());
            //    fd.ProcessVariables(parameterValues);
            //    fd.ProcessVariables(this.Current.FrameVariables);
            //    this.Current.Redirections.Add(fd);
            //}

            return null;
        }


        public override object VisitEnd(MFSHParser.EndContext context)
        {
            const String fcn = "VisitEnd";
            TraceMsg(context, fcn);

            //ParseBlock s = this.PopState();
            //switch (s)
            //{
            //    case MacroDefinition macroDef:
            //        this.mfsh.Defines.Add(macroDef.Name, macroDef);
            //        break;

            //    default:
            //        Error(fcn,
            //            context.Start.Line.ToString(),
            //            $"Unexpected '#end'");
            //        throw new Exception($"Unexpected '#end'");
            //}
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
            if (lines[0].StartsWith("\"\"\""))
                lines[0] = lines[0].Substring(3);
            if (lines[^1].EndsWith("\"\"\""))
                lines[^1] = lines[^1].Substring(0, lines[^1].Length - 3);
            while (String.IsNullOrWhiteSpace(lines[0]))
                lines.RemoveAt(0);
            while (String.IsNullOrWhiteSpace(lines[^1]))
                lines.RemoveAt(lines.Count - 1);

            StringBuilder sb = new StringBuilder();
            sb.Append(lines[0]);
            foreach (String line in lines.Skip(1))
            {
                sb.Append("\n");
                sb.Append(line);
            }

            return sb.ToString();
        }

        void Error(String fcn,
            String location,
            String msg)
        {
            String fullMsg = $"{this.SourceName}, line {location}. {msg}";
            this.mfsh.ConversionError("mfsh", fcn, fullMsg);
        }
    }
}
