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

namespace MFSH.Parser
{
    class MFSHVisitor : MFSHParserBaseVisitor<Object>
    {
        public bool DebugFlag { get; set; } = false;

        public Stack<StackFrame> state = new Stack<StackFrame>();
        public StackFrame Current => this.state.Peek();
        public string SourceName;
        MFsh mfsh;

        protected void AppendText(String text)
        {
            this.Current.Data.AppendText(text);
        }
        void PushState(StackFrame s)
        {
            this.state.Push(s);
        }

        StackFrame PopState()
        {
            StackFrame s = this.state.Peek();
            this.state.Pop();
            return s;
        }

        public MFSHVisitor(MFsh mfsh,
            string sourceName)
        {
            this.SourceName = sourceName;
            this.mfsh = mfsh;
            StackFrame f = new StackFrame();
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

        public override object VisitUse(MFSHParser.UseContext context)
        {
            const String fcn = "VisitUse";
            TraceMsg(context, fcn);
            String include = (String)this.VisitChildren(context.anyString());
            ProcessInclude(include);
            return null;
        }

        public override object VisitInclude(MFSHParser.IncludeContext context)
        {
            const String fcn = "VisitInclude";
            TraceMsg(context, fcn);
            String include = (String)this.VisitChildren(context.anyString());
            String text = ProcessInclude(include);
            if (text == null)
                return null;
            if (text.Length == 0)
                return null;
            if (text[^1] != '\n')
                text += '\n';
            this.AppendText(text);
            return null;
        }

        public override object VisitMacro(MFSHParser.MacroContext context)
        {
            const String fcn = "VisitMacro";
            TraceMsg(context, fcn);
            MacroDefinition macroDefinition = new MacroDefinition();
            this.PushState(macroDefinition);
            String[] names = context
                .NAME()
                .Select((a) => a.GetText())
                .ToArray();
            macroDefinition.Name = names[0];
            macroDefinition.Parameters.AddRange(names.Skip(1));

            if (this.Current.IncompatibleMacros.Contains(macroDefinition.Name))
            {
                this.Error(fcn,
                    context.Start.Line.ToString(),
                    $"Macro {macroDefinition.Name} has been marked as incompatible");
                return false;
            }

            var redirectContext = context.redirect();
            if (redirectContext != null)
            {
                if (redirectContext.JSONARRAY() != null)
                    macroDefinition.Data.RelativePathType = FileData.RedirType.Json;
                else if (redirectContext.TEXT() != null)
                    macroDefinition.Data.RelativePathType = FileData.RedirType.Text;
                else
                    throw new Exception("Unknown redirect type");
                macroDefinition.Data.RelativePath = (String)(this.Visit(redirectContext.singleString()));
            }

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

            if (this.Current.IncompatibleMacros.Contains(macroName))
                return null;
            this.Current.IncompatibleMacros.Add(macroName);
            if (this.Current.AppliedMacros.ContainsKey(macroName))
            {
                this.Error(fcn,
                    context.Start.Line.ToString(),
                    $"Incompatible macro {macroName} has already been applied");
                return false;
            }

            return null;
        }

        public override object VisitApply(MFSHParser.ApplyContext context)
        {
            const String fcn = "VisitApply";
            String macroName = context.NAME().GetText();
            string text;
            VariablesBlock parameterValues = new VariablesBlock();

            // Check to se if macro has already been applied.
            // return true to apply it, false to not apply it.
            bool CheckOnce()
            {
                bool onceFlag = (context.ONCE() != null);
                if (
                    (onceFlag) &&
                    (parameterValues.Count > 0)
                    )
                {
                    this.Error(fcn,
                        context.Start.Line.ToString(),
                        $"Attempt to apply macro {macroName} with once and variables (once does not work with parameterized macros)");
                    return false;
                }

                if (this.Current.AppliedMacros.TryGetValue(macroName, out ApplyInfo appliedMacro) == false)
                {
                    appliedMacro = new ApplyInfo
                    {
                        MacroName =  macroName,
                        OnceFlag = onceFlag
                    };
                    this.Current.AppliedMacros.Add(macroName, appliedMacro);
                    return true;
                }

                if ((onceFlag == true) && (appliedMacro.OnceFlag == true))
                    return false;
                if ((onceFlag == false) && (appliedMacro.OnceFlag == true))
                {
                    this.Error(fcn,
                        context.Start.Line.ToString(),
                        $"Attempt to call macro {macroName} with once = false, and previous call with once = true.");
                    return false;
                }
                if ((onceFlag == true) && (appliedMacro.OnceFlag == false))
                {
                    this.Error(fcn,
                        context.Start.Line.ToString(),
                        $"Attempt to call macro {macroName} with once = true, and previous call with once = false.");
                    return false;
                }

                return true;
            }

            TraceMsg(context, fcn);

            if (this.mfsh.Defines.TryGetValue(macroName, out MacroDefinition info) == false)
            {
                this.Error(fcn,
                    context.Start.Line.ToString(),
                    $"Macro {macroName} not found.");
                return null;
            }

            // Copy each incompatible macro from applied macro to current. Check to see if any of the new
            // incompatible entries match an already loaded macro (if so, generate error message)
            foreach (String incompatibleMacro in info.IncompatibleMacros)
            {
                if (this.Current.IncompatibleMacros.Contains(incompatibleMacro) == false)
                {
                    if (this.Current.AppliedMacros.ContainsKey(incompatibleMacro))
                    {
                        this.Error(fcn,
                            context.Start.Line.ToString(),
                            $"Incompatible macro {incompatibleMacro} has already been applied");
                        return false;
                    }
                    this.Current.IncompatibleMacros.Add(incompatibleMacro);
                }
            }

            foreach (ApplyInfo appliedMacro in info.AppliedMacros.Values)
            {
                if (this.Current.AppliedMacros.Keys.Contains(appliedMacro.MacroName) == false)
                {
                    if (this.Current.IncompatibleMacros.Contains(appliedMacro.MacroName))
                    {
                        this.Error(fcn,
                            context.Start.Line.ToString(),
                            $"Incompatible macro {appliedMacro.MacroName} has already been applied");
                        return false;
                    }
                    this.Current.AppliedMacros.Add(appliedMacro.MacroName, appliedMacro);
                }
            }

            if (LoadApplyParams(parameterValues, context.anyString(), info, context.Start) == false)
                return null;
            text = info.Data.Text();
            text = parameterValues.ReplaceText(text);
            text = this.Current.FrameVariables.ReplaceText(text);

            // See if macro was applied previously with once flag.
            if (CheckOnce() == false)
                return null;

            /*
             * Output of macro either goes to current output, or to a redirected target.
             */
            FileData macroOutput = this.Current.Data;
            if (String.IsNullOrEmpty(info.Data.RelativePath) == false)
            {
                String rPath = parameterValues.ReplaceText(info.Data.RelativePath);
                rPath = this.Current.FrameVariables.ReplaceText(rPath);
                macroOutput = new FileData
                {
                    RelativePath = rPath,
                    RelativePathType = info.Data.RelativePathType
                };
                this.Current.Redirections.Add(macroOutput);
            }
            macroOutput.AppendText(text);

            // Make a copy of redir and process variables. Don't
            // change original because it may be used again.
            foreach (FileData redir in info.Redirections)
            {

                FileData fd = new FileData
                {
                    RelativePath = redir.RelativePath,
                    RelativePathType = redir.RelativePathType
                };
                fd.AppendText(redir.Text());
                fd.ProcessVariables(parameterValues);
                fd.ProcessVariables(this.Current.FrameVariables);
                this.Current.Redirections.Add(fd);
            }

            return null;
        }


        public override object VisitEnd(MFSHParser.EndContext context)
        {
            const String fcn = "VisitEnd";
            TraceMsg(context, fcn);

            StackFrame s = this.PopState();
            switch (s)
            {
                case MacroDefinition macroDef:
                    this.mfsh.Defines.Add(macroDef.Name, macroDef);
                    break;

                default:
                    Error(fcn,
                        context.Start.Line.ToString(),
                        $"Unexpected '#end'");
                    throw new Exception($"Unexpected '#end'");
            }
            return null;
        }

        public override object VisitFshLine(MFSHParser.FshLineContext context)
        {
            const String fcn = "VisitFshLine";
            TraceMsg(context, fcn);

            String line = (String)this.VisitChildren(context.anyString());
            this.AppendText(line);
            this.AppendText("\n");
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


        public override object VisitProfile(MFSHParser.ProfileContext context)
        {
            String currentClass = context.NAME().GetText();
            this.Current.FrameVariables.Set("%Profile%", currentClass);
            String url = $"{this.mfsh.BaseUrl}/StructureDefinition/{currentClass}";
            this.Current.FrameVariables.Set("%ProfileUrl%", url);
            this.Current.IncompatibleMacros.Clear();
            return null;
        }

        #region Non Visitor Methods
        String FindInclude(String includeFile)
        {
            if (Path.IsPathRooted(includeFile))
            {
                if (File.Exists(includeFile) == true)
                    return includeFile;
            }

            if (this.mfsh.LocalDir != null)
            {
                String localFile = Path.Combine(this.mfsh.LocalDir, includeFile);
                if (File.Exists(localFile) == true)
                    return localFile;
            }

            if (File.Exists(includeFile) == true)
                return includeFile;
            foreach (String dir in this.mfsh.IncludeDirs)
            {
                String path = Path.GetFullPath(Path.Combine(dir, includeFile));
                if (File.Exists(path))
                    return path;
            }

            throw new Exception($"Include file '{includeFile}' does not exist");
        }

        String ProcessInclude(String includeFile)
        {
            const String fcn = "ProcessInclude";

            String includePath = this.FindInclude(includeFile);
            if (this.mfsh.Includes.Contains(includeFile))
                return null;

            this.mfsh.Includes.Add(includeFile);
            String fshText = File.ReadAllText(includePath);

            StackFrame frame = this.mfsh.SubParse(fshText,
                includeFile,
                Path.GetDirectoryName(includePath));
            if (frame.Redirections.Count > 0)
            {
                this.Error(fcn,
                    "",
                    $"Include file {includeFile} defined redirections which will not be saved");
            }
            return frame.Data.Text();
        }


        void Error(String fcn,
            String location,
            String msg)
        {
            String fullMsg = $"{this.SourceName}, line {location}. {msg}";
            this.mfsh.ConversionError("mfsh", fcn, fullMsg);
        }
        #endregion
    }
}
