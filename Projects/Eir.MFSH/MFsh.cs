using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Antlr4.Runtime.Tree;
using Eir.DevTools;
using Eir.MFSH;
using System.Text.RegularExpressions;
using Eir.MFSH.Manager;
using System.Xml.Serialization;

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
        VariablesBlock profileVariables = new VariablesBlock();

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
        public bool TryGetTextByRelativePath(String relativePath, out String text)
        {
            text = null;
            String absolutePath = Path.Combine(this.BaseOutputDir, relativePath);
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
            String mfshText = File.ReadAllText(path);

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
            this.ProcessFragments();
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

            String fragTempText = File.ReadAllText(this.FragTemplatePath);
            MIPreFsh fragTempCmds = this.Parser.ParseOne(fragTempText, this.FragTemplatePath);

            Dictionary<String, FileData> fragFileData = new Dictionary<string, FileData>();
            foreach (MIFragment frag in this.MacroMgr.Fragments())
                this.ProcessFragment(fragFileData, fragTempCmds, frag);
        }

        void ProcessFragment(Dictionary<String, FileData> fragFileData,
            MIPreFsh fragTempCmds,
            MIFragment frag)
        {
            const String fcn = "ProcessFragment";

            String relativePath = this.SetProfileVariables(frag.SourceFile);

            if (String.IsNullOrEmpty(frag.Parent) == true)
            {
                this.ConversionError(ClassName, fcn, $"Fragment {frag.Name} missing parent!");
                return;
            }

            if (fragFileData.TryGetValue(relativePath, out FileData fd) == false)
            {
                fd = new FileData();
                this.FileItems.Add(relativePath, fd);
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
            local.Insert(0, this.profileVariables);
            local.Insert(0, localVb);

            fd.AbsoluteOutputPath = Path.Combine(this.FragDir, relativePath);

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
            if (this.FileItems.TryGetValue(absolutePath, out fd))
                return false;
            fd = new FileData();
            fd.AbsoluteOutputPath = absolutePath;
            this.FileItems.Add(fd.AbsoluteOutputPath, fd);
            return true;
        }

        String SetProfileVariables(String relativePath)
        {
            String relativeFshPath = Path.Combine(
                Path.GetDirectoryName(relativePath),
                Path.GetFileNameWithoutExtension(relativePath) + ".fsh"
            );

            this.profileVariables = new VariablesBlock();
            {
                String baseRPath = relativePath;
                String baseName = Path.GetFileName(baseRPath);
                String baseNameNoExtension = Path.GetFileNameWithoutExtension(baseRPath);
                String baseDir = Path.GetDirectoryName(baseRPath);

                this.profileVariables.Set("%BasePath%", baseRPath);
                this.profileVariables.Set("%BaseDir%", baseDir);
                this.profileVariables.Set("%BaseName%", baseName);
                this.profileVariables.Set("%BaseNameNoExtension%", baseNameNoExtension);
                this.profileVariables.Set("%SavePath%", $"{relativeFshPath}");
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
            this.variableBlocks.Insert(0, this.profileVariables);

            if (this.GetFileData(relativeFshPath, this.variableBlocks, out FileData fd) == false)
            {
                this.ConversionError(ClassName, fcn, $"Output file {fd.AbsoluteOutputPath} already exists!");
                return;
            }

            this.Process(fsh.Items, fd, this.variableBlocks);

            this.variableBlocks = null;
            this.profileVariables = null;
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

                    case MIConditional conditional:
                        this.ProcessConditional(conditional, fd, variableBlocks);
                        i += 1;
                        break;

                    default:
                        throw new Exception("Internal error. Unknown MIXXX type");
                }
            }
        }

        Regex rProfile = new Regex("^[ \t]*Profile[ \t\n]*:[ \t\n]*([A-Za-z0-9\\-]+)");
        Regex rCodeSystem = new Regex("^[ \t]*CodeSystem[ \t\n]*:[ \t\n]*([A-Za-z0-9\\-]+)");
        Regex rValueSet = new Regex("^[ \t]*ValueSet[ \t\n]*:[ \t\n]*([A-Za-z0-9\\-]+)");
        Regex rExtension = new Regex("^[ \t]*Extension[ \t\n]*:[ \t\n]*([A-Za-z0-9\\-]+)");
        Regex rId = new Regex("^[ \t]*Id[ \t\n]*:[ \t\n]*([A-Za-z0-9\\-]+)");
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
                        this.profileVariables.Set("%Id%", vsName);
                        return;
                    }
                }
                {
                    Match m = this.rCodeSystem.Match(text.Line);
                    if (m.Success == true)
                    {
                        String csName = m.Groups[1].Value;
                        this.profileVariables.Set("%Id%", csName);
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
                        this.profileVariables.Set("%Id%", idName);
                        return;
                    }
                }
                {
                    Match m = this.rTitle.Match(text.Line);
                    if (m.Success == true)
                    {
                        String title = m.Groups[1].Value;
                        this.profileVariables.Set("%Title%", title);
                        return;
                    }
                }
            }

            String expandedText = variableBlocks.ReplaceText(text.Line);
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
            this.profileVariables.Set("%Id%", name);

            String profileUrl = $"{this.BaseUrl}/StructureDefinition/{name}";
            this.profileVariables.Set("%Url%", profileUrl);

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
            localVb.Add("%FId%", name);
            localVb.Add("%FParent%", frag.Parent);
            localVb.Add("%FTitle%", frag.Title);
            localVb.Add("%FDescription%", frag.Description);
            String fragmentUrl = $"{this.BaseUrl}/StructureDefinition/{name}";
            localVb.Add("%FUrl%", fragmentUrl);

            return localVb;
        }


        void ProcessApply(MIApply apply,
            FileData fd,
            List<VariablesBlock> variableBlocks)
        {
            const String fcn = "ProcessApply";

            List<VariablesBlock> local = new List<VariablesBlock>();
            local.AddRange(variableBlocks);

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

            if (macro.SingleFlag && macro.AppliedFlag)
                return;

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
