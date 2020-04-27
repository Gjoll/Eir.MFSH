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

        public Stack<FileData> state = new Stack<FileData>();

        public FileData Current => this.state.Peek();
        public string SourceName;
        MFsh mfsh;

        protected void AppendText(String text)
        {
            this.Current.AppendText(text);
        }
        void PushState(FileData s)
        {
            this.state.Push(s);
        }

        FileData PopState()
        {
            FileData s = this.state.Peek();
            this.state.Pop();
            return s;
        }

        public MFSHVisitor(MFsh mfsh,
            string sourceName)
        {
            this.SourceName = sourceName;
            this.mfsh = mfsh;
            this.PushState(new FileData());
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

            DefineInfo s = new DefineInfo();
            this.PushState(s);
            String[] names = context
                .NAME()
                .Select((a) => a.GetText())
                .ToArray();
            s.Name = names[0];
            s.Parameters.AddRange(names.Skip(1));

            var redirectContext = context.redirect();
            if (redirectContext != null)
                s.RedirectDataPath = (String)(this.Visit(redirectContext.singleString()));
            return null;
        }

        bool LoadApplyParams(VariablesBlock variablesBlock,
            IEnumerable<MFSHParser.AnyStringContext> varsContext,
            DefineInfo info,
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
                    start,
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

        public override object VisitApply(MFSHParser.ApplyContext context)
        {
            const String fcn = "VisitApply";
            TraceMsg(context, fcn);

            String macroName = context.NAME().GetText();
            using (VariablesBlock b = new VariablesBlock($"macro {macroName}", this.mfsh.Variables))
            {
                String tracePath = this.mfsh.Variables.StackTrace();
                Debug.Assert(macroName != "GraphNode");
                if (this.mfsh.Defines.TryGetValue(macroName, out DefineInfo info) == false)
                {
                    this.Error(fcn,
                        context.Start,
                        $"Macro {macroName} not found.");
                    return null;
                }

                if (LoadApplyParams(b, context.anyString(), info, context.Start) == false)
                    return null;


                string text = info.GetText();
                text = this.mfsh.Variables.ReplaceText(text);

                /*
                 * Output of macro either goes to current output, or to a redirected target.
                 * If redirected, the redirection path is evaluated now, so any variables
                 * can be expanded.
                 */
                FileData macroOutput = this.Current;
                if (String.IsNullOrEmpty(info.RedirectDataPath) == false)
                {
                    String rPath = this.mfsh.Variables.ReplaceText(info.RedirectDataPath);
                    
                    // Create new file data if a file data with this relative path
                    // name does not already exist.
                    if (this.mfsh.FileItems.TryGetValue(rPath, out macroOutput) == false)
                    {
                        macroOutput = new JsonArrayData
                        {
                            RelativePath = rPath
                        };
                        this.mfsh.FileItems.Add(rPath, macroOutput);
                    }
                }
                macroOutput.AppendText(text);
                return null;
            }
        }


        public override object VisitEnd(MFSHParser.EndContext context)
        {
            const String fcn = "VisitEnd";
            TraceMsg(context, fcn);

            FileData s = this.PopState();
            switch (s)
            {
                case DefineInfo defineInfo:
                    this.mfsh.Defines.Add(defineInfo.Name, defineInfo);
                    break;

                default:
                    Error(fcn,
                        context.Start,
                        $"Unexpected '#end'");
                    break;
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
            this.mfsh.Variables.Current.Remove("%CurrentClass%");
            this.mfsh.Variables.Current.Add("%CurrentClass%", currentClass);
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
            String includePath = this.FindInclude(includeFile);
            if (this.mfsh.Includes.Contains(includeFile))
                return null;

            this.mfsh.Includes.Add(includeFile);
            String fshText = File.ReadAllText(includePath);

            string includeFileText = this.mfsh.SubParse(fshText,
                includeFile,
                Path.GetDirectoryName(includePath));
            return includeFileText;
        }


        void Error(String fcn,
            IToken start,
            String msg)
        {
            String fullMsg = $"{this.SourceName}, line {start.Line}. {msg}";
            this.mfsh.ConversionError("mfsh", fcn, fullMsg);
        }
        #endregion
    }
}
