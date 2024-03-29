﻿using Eir.DevTools;
using Eir.MFSH.Manager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Eir.MFSH
{
    public class MFsh : ConverterBase
    {
        private bool skipRedirects = false;
        private const String ClassName = "MFsh";
        private FileCleaner fc = new FileCleaner();
        public bool DebugFlag { get; set; } = false;
        public ParseManager Parser { get; }

        List<String> usings;

        /// <summary>
        /// Handles storing and retrieving all macros.
        /// </summary>
        public ApplicableManager MacroMgr { get; }

        HashSet<String> appliedMacros = new HashSet<string>();
        HashSet<String> incompatibleMacros = new HashSet<string>();
        Stack<MIApply> applyStack = new Stack<MIApply>();

        public string BaseInputDir { get; set; }
        public string BaseOutputDir { get; set; }

        public bool Frags { get; set; } = false;
        public string FragDir { get; set; }
        public String FragTemplatePath { get; set; }

        // Keep track of include files so we dont end up in recursive loop.
        List<String> sources = new List<string>();

        public const String FSHSuffix = ".fsh";
        public const String MFSHSuffix = ".mfsh";
        public const String MINCSuffix = ".minc";

        public String BaseUrl { get; set; }

        /// <summary>
        /// List of all variable blocks.
        /// A new one of these is created on every file.
        /// </summary>
        List<VariablesBlock> variableBlocks;

        /// <summary>
        /// Global variables block. Variables that do not change from profile to profile are stored here.
        /// </summary>
        public VariablesBlock GlobalVars = new VariablesBlock();

        /// <summary>
        /// Variables that are local to a profile (or a  file)
        /// </summary>
        public VariablesBlock ProfileVariables = new VariablesBlock();

        /// <summary>
        /// List of all variables block in action.
        /// </summary>
        public Dictionary<String, FileData> FileItems =
            new Dictionary<String, FileData>();

        public MFsh()
        {
            this.Parser = new ParseManager(this);
            this.MacroMgr = new ApplicableManager(this);
        }

        public override void ConversionError(String className, String method, String msg)
        {
            base.ConversionError(className, method, msg);
        }

        public override void ConversionInfo(String className, String method, String msg)
        {
            base.ConversionInfo(className, method, msg);
        }

        public override void ConversionWarn(String className, String method, String msg)
        {
            base.ConversionWarn(className, method, msg);
        }


        /// <summary>
        /// Turn on file cleaning. if on, then files in output dir that are not updated will
        /// be deleted.
        /// </summary>
        public void FileClean(String path, String fileFilter)
        {
            String fullPath = Path.GetFullPath(path);
            this.fc.Add(fullPath, fileFilter);
        }


        /// <summary>
        /// Only valid after Process() called.
        /// </summary>
        /// <param name="relativeFileName"></param>
        /// <returns></returns>
        public bool TryGetFragTextByRelativePath(String relativePath, out String text)
        {
            text = null;
            String absolutePath = Path.Combine(this.FragDir.ToUpper(), relativePath.ToUpper());
            if (this.FileItems.TryGetValue(absolutePath, out FileData fd) == false)
                return false;
            text = fd.Text;
            return true;
        }

        /// <summary>
        /// Only valid after Process() called.
        /// </summary>
        /// <param name="relativeFileName"></param>
        /// <returns></returns>
        public bool TryGetTextByRelativePath(String relativePath, out String text)
        {
            text = null;
            String absolutePath = Path.Combine(this.BaseOutputDir.ToUpper(), relativePath.ToUpper());
            if (this.FileItems.TryGetValue(absolutePath, out FileData fd) == false)
                return false;
            text = fd.Text;
            return true;
        }

        /// <summary>
        /// Process single file.
        /// </summary>
        void LoadFile(String path)
        {
            const String fcn = "ProcessFile";

            if (String.IsNullOrEmpty(this.BaseInputDir) == true)
                throw new Exception($"BaseInputDir not set");
            path = Path.GetFullPath(path);

            String relativePath = path.Substring(this.BaseInputDir.Length);
            if (relativePath.StartsWith("\\"))
                relativePath = relativePath.Substring((1));

            if (path.StartsWith(this.BaseInputDir) == false)
                throw new Exception("Internal error. Path does not start with correct base path");

            this.ConversionInfo(this.GetType().Name, fcn, $"Loading file {path}");
            String mfshText = ReadAllText(path);

            this.Parser.ParseOne(mfshText, relativePath);
        }

        void LoadDir(String path)
        {
            const String fcn = "LoadDir";

            if (String.IsNullOrEmpty(this.BaseInputDir) == true)
                throw new Exception($"BaseInputDir not set");

            this.ConversionInfo(this.GetType().Name, fcn, $"Loading directory {path}");
            foreach (String subDir in Directory.GetDirectories(path))
                this.LoadDir(subDir);

            foreach (String file in Directory.GetFiles(path, $"*{MFsh.MFSHSuffix}"))
                this.LoadFile(file);
            foreach (String file in Directory.GetFiles(path, $"*{MFsh.MINCSuffix}"))
                this.LoadFile(file);
        }

        public void Load(String path)
        {
            const String fcn = "Load";
            try
            {
                if (Directory.Exists(path))
                    this.LoadDir(path);
                else if (File.Exists(path))
                    this.LoadFile(path);
                else
                    throw new Exception($"Path {path} does not exist");
            }
            catch (Exception err)
            {
                String fullMsg = $"Error loading {Path.GetFileName(path)}. '{err.Message}'";
                this.ConversionError(ClassName, fcn, fullMsg);
            }
        }

        /// <summary>
        /// Process all files in indicated dir and sub dirs.
        /// </summary>
        public void Process()
        {
            if (String.IsNullOrEmpty(this.BaseUrl) == true)
                throw new Exception($"BaseUrl not set");
            this.ProcessPreFsh();
            if (this.Frags)
                this.ProcessFragments();
        }

        /// <summary>
        /// Read all text. Make sure that last char of file is a line break, adding one if necessary.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        String ReadAllText(String path)
        {
            //const String fcn = "ReadAllText";

            String retVal = File.ReadAllText(path);
            // If last line if white space with no end of line, remove it.
            while (
                (retVal.Length > 0) &&
                ((retVal[retVal.Length - 1] == ' ') || (retVal[retVal.Length - 1] == '\t'))
            )
                retVal = retVal.Substring(0, retVal.Length - 1);

            if (retVal[retVal.Length - 1] != '\n')
                retVal += "\r\n";
            return retVal;
        }

        void ProcessFragments()
        {
            const String fcn = "ProcessFragments";

            if (String.IsNullOrEmpty(this.FragDir))
                return;

            if (String.IsNullOrEmpty(this.FragTemplatePath) == true)
            {
                this.ConversionError(ClassName, fcn, $"Fragment template not set!");
                return;
            }

            if (File.Exists(this.FragTemplatePath) == false)
            {
                this.ConversionError(ClassName, fcn, $"Fragment template {this.FragTemplatePath} does not exist!");
                return;
            }

            String fragTempText = ReadAllText(this.FragTemplatePath);
            MIPreFsh fragTempCmds = this.Parser.ParseOne(fragTempText, this.FragTemplatePath);

            foreach (MIFragment frag in this.MacroMgr.Fragments())
                this.ProcessFragment(fragTempCmds, frag);
        }

        void ProcessFragment(MIPreFsh fragTempCmds,
            MIFragment frag)
        {
            const String fcn = "ProcessFragment";

            this.appliedMacros.Clear();
            this.incompatibleMacros.Clear();

            String relativePath = this.SetProfileVariables(frag.SourceFile);

            if (String.IsNullOrEmpty(frag.Parent) == true)
            {
                this.ConversionError(ClassName, fcn, $"Fragment {frag.Name} missing parent!");
                return;
            }

            if (relativePath.ToUpper().StartsWith(@"FRAGMENTS\"))
                relativePath = relativePath.Substring(10);
            String absolutePath = Path.Combine(this.FragDir, relativePath);
            absolutePath = Path.GetFullPath(absolutePath);

            if (this.FileItems.TryGetValue(absolutePath.ToUpper().Trim(), out FileData fd) == false)
            {
                fd = new FileData();
                fd.AbsoluteOutputPath = absolutePath;
                this.FileItems.Add(absolutePath.ToUpper().Trim(), fd);
            }
            else
            {
                fd.AppendLine("");
                fd.AppendLine("");
                fd.AppendLine("");
            }

            VariablesBlock localVb = StartNewFrag(frag, out String name);
            List<VariablesBlock> local = new List<VariablesBlock>();
            local.Insert(0, this.GlobalVars);
            local.Insert(0, this.ProfileVariables);
            local.Insert(0, localVb);

            this.skipRedirects = false;
            this.Process(fragTempCmds.Items, fd, local);
            this.skipRedirects = true;

            this.Process(frag.Items, fd, local);
        }

        void ProcessPreFsh()
        {
            this.skipRedirects = false;
            foreach (MIPreFsh fsh in this.Parser.Fsh)
                this.Process(fsh);
        }

        /// <summary>
        /// Get/Create file data item.
        /// If item with relative path already existsm, return that, 
        /// create a new one.
        /// </summary>
        /// <returns>true if new one created</returns>
        bool GetFileData(String relativePath,
            List<VariablesBlock> variableBlocks,
            out FileData fd)
        {
            relativePath = variableBlocks.ReplaceText(relativePath);
            String absolutePath = Path.Combine(this.BaseOutputDir, relativePath);
            absolutePath = Path.GetFullPath(absolutePath);
            if (this.FileItems.TryGetValue(absolutePath.ToUpper().Trim(), out fd))
                return false;
            //Debug.Assert(absolutePath.ToUpper().EndsWith("ABNORMALITYARCHITECTURALDISTORTION.APIBUILD") == false);
            fd = new FileData();
            fd.AbsoluteOutputPath = absolutePath;
            this.FileItems.Add(absolutePath.ToUpper().Trim(), fd);
            return true;
        }

        String SetProfileVariables(String relativePath)
        {
            String relativeFshPath = Path.Combine(
                Path.GetDirectoryName(relativePath),
                Path.GetFileNameWithoutExtension(relativePath) + ".fsh"
            );

            this.ProfileVariables = new VariablesBlock();
            {
                String baseRPath = relativePath;
                String baseName = Path.GetFileName(baseRPath);
                String baseNameNoExtension = Path.GetFileNameWithoutExtension(baseRPath);
                String baseDir = Path.GetDirectoryName(baseRPath);

                this.ProfileVariables.Set("%BasePath%", baseRPath);
                this.ProfileVariables.Set("%BaseDir%", baseDir);
                this.ProfileVariables.Set("%BaseName%", baseNameNoExtension);
                this.ProfileVariables.Set("%SavePath%", $"{relativeFshPath}");
            }
            return relativeFshPath;
        }

        public void Process(MIPreFsh fsh)
        {
            const String fcn = "Process";

            if (Path.GetExtension(fsh.RelativePath).ToLower() == MINCSuffix)
                return;

            this.usings = fsh.Usings;
            String relativeFshPath = this.SetProfileVariables(fsh.RelativePath);
            this.variableBlocks = new List<VariablesBlock>();
            this.variableBlocks.Insert(0, this.GlobalVars);
            this.variableBlocks.Insert(0, this.ProfileVariables);

            if (this.GetFileData(relativeFshPath, this.variableBlocks, out FileData fd) == false)
            {
                this.ConversionError(ClassName, fcn, $"Output file {fd.AbsoluteOutputPath} already exists!");
                return;
            }

            this.Process(fsh.Items, fd, this.variableBlocks);

            this.variableBlocks = null;
            this.ProfileVariables = null;
        }

        void Process(List<MIBase> inputItems,
            FileData fd,
            List<VariablesBlock> variableBlocks)
        {
            Int32 i = 0;
            while (i < inputItems.Count)
            {
                MIBase b = inputItems[i];
                switch (b)
                {
                    case MIText text:
                        this.ProcessText(text, fd, variableBlocks);
                        i += 1;
                        break;

                    case MIApply apply:
                        this.ProcessApply(apply, fd, variableBlocks);
                        i += 1;
                        break;

                    case MIIncompatible incompatible:
                        this.ProcessIncompatible(incompatible, fd, variableBlocks);
                        i += 1;
                        break;

                    case MISet set:
                        this.ProcessSet(set, fd, variableBlocks);
                        i += 1;
                        break;

                    case MICall call:
                        this.ProcessCall(call, fd, variableBlocks);
                        i += 1;
                        break;

                    case MIConditional conditional:
                        this.ProcessConditional(conditional, fd, variableBlocks);
                        i += 1;
                        break;

                    default:
                        throw new Exception("Internal error. Unknown MIXXX type");
                }
            }
        }

        static Regex RegExpName(String name) =>
            new Regex($"^[ \t]*{name}[ \t\n]*:[ \t\n]*([A-Za-z0-9_\\-]+)");
        Regex rProfile = RegExpName("Profile");
        Regex rCodeSystem = RegExpName("CodeSystem");
        Regex rValueSet = RegExpName("ValueSet");
        Regex rExtension = RegExpName("Extension");
        Regex rId = RegExpName("Id");
        Regex rTitle = new Regex("^[ \t]*Title[ \t\n]*:[ \t\n]*\"([^\"]+)\"");

        void ProcessText(MIText text,
            FileData fd,
            List<VariablesBlock> variableBlocks)
        {
            void ProcessHeader()
            {
                {
                    Match m = this.rProfile.Match(text.Line);
                    if (m.Success == true)
                    {
                        String profileName = m.Groups[1].Value;
                        this.StartNewProfile(profileName);
                        return;
                    }
                }
                {
                    Match m = this.rValueSet.Match(text.Line);
                    if (m.Success == true)
                    {
                        String vsName = m.Groups[1].Value;
                        this.ProfileVariables.Set("%ValueSetId%", vsName);
                        this.ProfileVariables.Set("%ValueSetName%", vsName);
                        this.ProfileVariables.Set("%Id%", vsName);
                        return;
                    }
                }
                {
                    Match m = this.rCodeSystem.Match(text.Line);
                    if (m.Success == true)
                    {
                        String csName = m.Groups[1].Value;
                        this.ProfileVariables.Set("%CodeSystemName%", csName);
                        this.ProfileVariables.Set("%CodeSystemId%", csName);
                        this.ProfileVariables.Set("%Id%", csName);
                        return;
                    }
                }
                {
                    Match m = this.rExtension.Match(text.Line);
                    if (m.Success == true)
                    {
                        String extensionName = m.Groups[1].Value;
                        this.StartNewExtension(extensionName);
                        return;
                    }
                }
                {
                    Match m = this.rId.Match(text.Line);
                    if (m.Success == true)
                    {
                        String idName = m.Groups[1].Value;
                        if (this.ProfileVariables.IsSet("%ProfileId%"))
                            this.ProfileVariables.Set("%ProfileId%", idName);
                        if (this.ProfileVariables.IsSet("%CodeSystemId%"))
                            this.ProfileVariables.Set("%CodeSystemId%", idName);
                        if (this.ProfileVariables.IsSet("%ValueSetId%"))
                            this.ProfileVariables.Set("%ValueSetId%", idName);
                        this.ProfileVariables.Set("%Id%", idName);
                        return;
                    }
                }
                {
                    Match m = this.rTitle.Match(text.Line);
                    if (m.Success == true)
                    {
                        String title = m.Groups[1].Value;
                        this.ProfileVariables.Set("%Title%", title);
                        return;
                    }
                }
            }

            String expandedText = variableBlocks.ReplaceText(text.Line);
            //if (expandedText.Contains("* bodySite 1..1"))
            //    Debugger.Break();

            fd.Append(expandedText);
            ProcessHeader();
        }

        public String ApplyLongStackTrace()
        {
            if (this.applyStack.Count == 0)
                return "";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("");
            sb.AppendLine("Macro Stack Trace.");
            foreach (MIApply applyStackFrame in this.applyStack)
                sb.Append(applyStackFrame.ToString());
            return sb.ToString();
        }

        public String ApplyShortStackTrace()
        {
            if (this.applyStack.Count == 0)
                return "";
            StringBuilder sb = new StringBuilder();
            foreach (MIApply applyStackFrame in this.applyStack)
            {
                sb.Append($@"{applyStackFrame.SourceFile}#{applyStackFrame.LineNumber}\n");
            }
            return sb.ToString();
        }

        void StartNewItem(String name)
        {
            // %Id% defaults to profile unless explicitly set (later)
            this.ProfileVariables.Set("%ProfileId%", name);
            this.ProfileVariables.Set("%Id%", name);

            String profileUrl = $"{this.BaseUrl}/StructureDefinition/{name}";
            this.ProfileVariables.Set("%Url%", profileUrl);

            this.appliedMacros.Clear();
            this.incompatibleMacros.Clear();
        }

        void StartNewExtension(String extensionName)
        {
            this.StartNewItem(extensionName);
        }

        void StartNewProfile(String profileName)
        {
            this.StartNewItem(profileName);
        }

        VariablesBlock StartNewFrag(MIFragment frag,
            out String name)
        {
            name = frag.Name;
            if (name.Contains('.'))
                name = name.Substring(name.LastIndexOf('.') + 1);

            VariablesBlock localVb = new VariablesBlock();
            localVb.Add("%Id%", name);
            localVb.Add("%FragmentId%", name);
            localVb.Add("%FParent%", frag.Parent);
            localVb.Add("%FTitle%", frag.Title);
            localVb.Add("%FDescription%", frag.Description);
            String fragmentUrl = $"{this.BaseUrl}/StructureDefinition/{name}";
            localVb.Add("%FUrl%", fragmentUrl);

            //this.appliedMacros.Clear();
            //this.incompatibleMacros.Clear();
            return localVb;
        }


        void ProcessApply(MIApply apply,
            FileData fd,
            List<VariablesBlock> variableBlocks)
        {
            const String fcn = "ProcessApply";

            List<VariablesBlock> local = new List<VariablesBlock>();
            local.AddRange(variableBlocks);

            apply.ApplyCount += 1;
            if ((apply.OnceFlag) && (apply.ApplyCount > 1))
                return;

            //Debug.Assert(apply.Name.EndsWith("DefineFragment") == false);
            if (this.MacroMgr.TryGetItem(apply.Usings, apply.Name, out MIApplicable applicableItem) == false)
            {
                String fullMsg = $"{apply.SourceFile}, line {apply.LineNumber} Macro {apply.Name} not found.";
                fullMsg += this.ApplyLongStackTrace();
                this.ConversionError(ClassName, fcn, fullMsg);
                return;
            }
            switch (applicableItem)
            {
                case MIMacro macro:
                    this.ProcessApplyMacro(apply, fd, macro, local);
                    break;

                case MIFragment frag:
                    this.ProcessApplyFrag(apply, fd, frag, local);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        void ProcessApplyMacro(MIApply apply,
            FileData fd,
            MIMacro macro,
            List<VariablesBlock> local)
        {
            const String fcn = "ProcessApplyMacro";

            if (macro.Parameters.Count != apply.Parameters.Count)
            {
                String fullMsg = $"{apply.SourceFile}, line {apply.LineNumber} Macro {apply.Name} requires {macro.Parameters.Count} parameters, called with {apply.Parameters.Count}.";
                fullMsg += this.ApplyLongStackTrace();
                this.ConversionError(ClassName, fcn, fullMsg);
                return;
            }

            macro.AppliedFlag = true;
            bool firstFlag = false;
            if (this.appliedMacros.Contains(apply.Name) == false)
            {
                this.appliedMacros.Add(apply.Name);
                firstFlag = true;
            }

            if ((macro.OnceFlag == true) && (firstFlag == false))
                return;

            if (this.incompatibleMacros.Contains(apply.Name))
            {
                String fullMsg = $"{apply.SourceFile}, line {apply.LineNumber} Macro {apply.Name} has been marked as incompatible with this profile";
                fullMsg += this.ApplyLongStackTrace();
                this.ConversionError(ClassName, fcn, fullMsg);
                return;
            }

            local.Insert(0, macro.ApplicableVariables);
            VariablesBlock vbParameters = new VariablesBlock();
            for (Int32 i = 0; i < apply.Parameters.Count; i++)
            {
                String pName = macro.Parameters[i];
                String pValue = apply.Parameters[i];
                if (this.variableBlocks != null)
                    pValue = this.variableBlocks.ReplaceText(pValue);
                vbParameters.Add(pName, pValue);
            }

            vbParameters.Add("%ApplySourceFile%", apply.SourceFile.Replace("\\", "/"));
            vbParameters.Add("%ApplyLineNumber%", apply.LineNumber.ToString());

            local.Insert(0, vbParameters);

            FileData macroData = fd;
            if (String.IsNullOrEmpty(macro.Redirect) == false)
            {
                if (this.skipRedirects == true)
                    return;
                this.GetFileData(macro.Redirect, local, out macroData);
            }

            this.applyStack.Push(apply);                    // this is for stack tracing during errors
            vbParameters.Add("%ApplyStackFrame%", this.ApplyShortStackTrace().Replace("\\", "/"));
            this.Process(macro.Items, macroData, local);
            this.applyStack.Pop();
        }

        void ProcessApplyFrag(MIApply apply,
            FileData fd,
            MIFragment frag,
            List<VariablesBlock> local)
        {
            const String fcn = "ProcessApplyMacro";

            if (apply.Parameters.Count != 0)
            {
                String fullMsg = $"{apply.SourceFile}, line {apply.LineNumber} Fragment {apply.Name} takes no parameters.";
                fullMsg += this.ApplyLongStackTrace();
                this.ConversionError(ClassName, fcn, fullMsg);
                return;
            }

            bool firstFlag = false;
            if (this.appliedMacros.Contains(apply.Name) == false)
            {
                this.appliedMacros.Add(apply.Name);
                firstFlag = true;
            }

            if ((frag.OnceFlag == true) && (firstFlag == false))
                return;

            if (this.incompatibleMacros.Contains(apply.Name))
            {
                String fullMsg = $"{apply.SourceFile}, line {apply.LineNumber} {apply.Name} has been marked as incompatible with this profile";
                fullMsg += this.ApplyLongStackTrace();
                this.ConversionError(ClassName, fcn, fullMsg);
                return;
            }

            VariablesBlock localVb = StartNewFrag(frag, out String name);
            List<VariablesBlock> fragLocal = new List<VariablesBlock>();
            fragLocal.AddRange(local);
            fragLocal.Insert(0, localVb);

            FileData macroData = fd;
            this.applyStack.Push(apply);                    // this is for stack tracing during errors
            this.Process(frag.Items, macroData, fragLocal);
            this.applyStack.Pop();
        }

        void ProcessConditional(MIConditional conditional,
                FileData fd,
                List<VariablesBlock> variableBlocks)
        {
            foreach (MIConditional.Condition c in conditional.Conditions)
            {
                if (c.State.IsTrue(variableBlocks))
                {
                    this.Process(c.Items, fd, variableBlocks);
                    return;
                }
            }
        }

        void ProcessIncompatible(MIIncompatible incompatible,
            FileData fd,
            List<VariablesBlock> variableBlocks)
        {
            const String fcn = "ProcessIncompatible";

            if (this.MacroMgr.TryGetItem(this.usings, incompatible.Name, out MIApplicable macro) == false)
            {
                String fullMsg = $"{incompatible.SourceFile}, line {incompatible.LineNumber} Macro {incompatible.Name} not found.";
                fullMsg += this.ApplyLongStackTrace();
                this.ConversionError(ClassName, fcn, fullMsg);
                return;
            }

            if (this.incompatibleMacros.Contains(incompatible.Name) == true)
                return;

            if (this.appliedMacros.Contains(incompatible.Name) == true)
            {
                String fullMsg = $"{incompatible.SourceFile}, line {incompatible.LineNumber} Macro {incompatible.Name} is incompatible and has already been applied.";
                fullMsg += this.ApplyLongStackTrace();
                this.ConversionError(ClassName, fcn, fullMsg);
                return;
            }

            if (this.incompatibleMacros.Add(incompatible.Name) == true)
                return;
        }

        void ProcessCall(MICall call,
            FileData fd,
            List<VariablesBlock> variableBlocks)
        {
            const String fcn = "ProcessCall";

            String command = variableBlocks.ReplaceText(call.Name);
            command = Path.Combine(BaseInputDir, command);
            StringBuilder arguments = new StringBuilder();
            foreach (String param in call.Parameters)
                arguments.Append($"{variableBlocks.ReplaceText(param)} ");
            try
            {
                async Task ReadOutAsync(Process p)
                {
                    do
                    {
                        String s = await p.StandardOutput.ReadLineAsync();
                        s = s?.Replace("\r", "")?.Replace("\n", "")?.Trim();
                        fd.Append(s);
                    } while (p.StandardOutput.EndOfStream == false);
                }

                async Task ReadErrAsync(Process p)
                {
                    do
                    {
                        String s = await p.StandardError.ReadLineAsync();
                        s = s?.Replace("\r", "")?.Replace("\n", "")?.Trim();
                        if (String.IsNullOrEmpty(s) == false)
                            ConversionError(ClassName, fcn, s);
                    } while (p.StandardError.EndOfStream == false);
                }

                if (File.Exists(command) == false)
                {
                    ConversionError(ClassName, fcn, $"Command {command} not found.");
                    return;
                }

                using (Process p = new Process())
                {
                    p.StartInfo.FileName = command;
                    p.StartInfo.Arguments = arguments.ToString();
                    p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.RedirectStandardError = true;
                    p.StartInfo.WorkingDirectory = BaseInputDir;
                    p.Start();
                    Task errTask = ReadErrAsync(p);
                    Task outTask = ReadOutAsync(p);
                    p.WaitForExit(); // Waits here for the process to exit.    }
                    errTask.Wait();
                    outTask.Wait();
                    if (p.ExitCode < 0)
                        ConversionError(ClassName, fcn, $"Command {command} returned exit code {p.ExitCode}");
                }
            }
            catch (Exception err)
            {
                String fullMsg = $"Error processing command {command}. '{err.Message}'";
                this.ConversionError(ClassName, fcn, fullMsg);
            }
        }

        void ProcessSet(MISet set,
            FileData fd,
            List<VariablesBlock> variableBlocks)
        {
            //const String fcn = "ProcessSet";

            String expandedValue = variableBlocks.ReplaceText(set.Value);
            this.ProfileVariables.Set(set.Name, expandedValue);
        }

        /// <summary>
        /// Write all files back to disk.
        /// </summary>
        public void SaveAll()
        {
            const String fcn = "SaveAll";

            void Save(String outputPath, String text)
            {
                text = text.Trim();
                if (text.Length == 0)
                    return;

                // Mark file as updated. If file cleaning on, this will stop file
                // from being deleted during clean phase.
                this.fc.Mark(outputPath);

                String dir = Path.GetDirectoryName(outputPath);
                if (Directory.Exists(dir) == false)
                    Directory.CreateDirectory(dir);

                if (FileTools.WriteModifiedText(outputPath, text) == true)
                    this.ConversionInfo(this.GetType().Name,
                        fcn,
                        $"Saving {Path.GetFileName(outputPath)}");
            }

            this.ConversionInfo(this.GetType().Name, fcn, $"Saving all processed files");
            foreach (FileData f in this.FileItems.Values)
            {
                string outText = f.Text.ToString();
                Save(f.AbsoluteOutputPath, outText);
            }

            this.fc.DeleteUnMarkedFiles();
        }
    }
}
