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

namespace Eir.MFSH
{
    public class MFsh : ConverterBase
    {
        private const String ClassName = "MFsh";
        private FileCleaner fc = new FileCleaner();
        public bool DebugFlag { get; set; } = false;
        public ParseManager Parser { get; }

        List<String> usings;

        /// <summary>
        /// Handles storing and retrieving all macros.
        /// </summary>
        public MacroManager Macros { get; }

        HashSet<String> appliedMacros = new HashSet<string>();
        HashSet<String> incompatibleMacros = new HashSet<string>();
        Stack<MIApply> applyStack = new Stack<MIApply>();

        public string BaseInputDir
        {
            get => this.baseInputDir;
            set => this.baseInputDir = Path.GetFullPath(value);
        }
        private string baseInputDir;

        public string BaseOutputDir
        {
            get => this.baseOutputDir;
            set => this.baseOutputDir = Path.GetFullPath(value);
        }
        private string baseOutputDir;

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
            this.Macros = new MacroManager(this);
        }

        /// <summary>
        /// Turn on file cleaning. if on, then files in output dir that are not updated will
        /// be deleted.
        /// </summary>
        public void FileClean(String path, String fileFilter) => this.fc.Add(Path.GetFullPath(path), fileFilter);

        /// <summary>
        /// Only valid after Process() called.
        /// </summary>
        /// <param name="relativeFileName"></param>
        /// <returns></returns>
        public bool TryGetText(String relativePath, out String text)
        {
            text = null;
            if (this.FileItems.TryGetValue(relativePath, out FileData fd) == false)
                return false;
            text = fd.Text.ToString();
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

            if (path.StartsWith(BaseInputDir) == false)
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
            foreach (MIPreFsh fsh in this.Parser.Fsh)
                this.Process(fsh);
        }

        /// <summary>
        /// Get/Create file data item.
        /// If item with relative path already existsm, return that, 
        /// create a new one.
        /// </summary>
        /// <returns>true if new one created</returns>
        bool GetFileData(String relativePath, out FileData fd)
        {
            relativePath = this.variableBlocks.ReplaceText(relativePath);
            if (this.FileItems.TryGetValue(relativePath, out fd))
                return false;
            fd = new FileData();
            fd.RelativePath = relativePath;
            this.FileItems.Add(relativePath, fd);
            return true;
        }

        public void Process(MIPreFsh fsh)
        {
            const String fcn = "Process";

            if (Path.GetExtension(fsh.RelativePath).ToLower() == MINCSuffix)
                return;

            String relativeFshPath = Path.Combine(
                Path.GetDirectoryName(fsh.RelativePath),
                Path.GetFileNameWithoutExtension(fsh.RelativePath) + ".fsh"
            );
            this.usings = fsh.Usings;
            this.profileVariables = new VariablesBlock();
            {
                String baseRPath = fsh.RelativePath;
                String baseName = Path.GetFileName(baseRPath);
                String baseNameNoExtension = Path.GetFileNameWithoutExtension(baseRPath);
                String baseDir = Path.GetDirectoryName(baseRPath);

                profileVariables.Set("%BasePath%", baseRPath);
                profileVariables.Set("%BaseDir%", baseDir);
                profileVariables.Set("%BaseName%", baseName);
                profileVariables.Set("%BaseNameNoExtension%", baseNameNoExtension);
                profileVariables.Set("%SavePath%", $"{relativeFshPath}");
            }
            this.variableBlocks = new List<VariablesBlock>();
            variableBlocks.Insert(0, this.GlobalVars);
            variableBlocks.Insert(0, profileVariables);

            if (GetFileData(relativeFshPath, out FileData fd) == false)
            {
                this.ConversionError(ClassName, fcn, $"Output file {fd.RelativePath} already exists!");
                return;
            }

            this.Process(fsh.Items, fd, variableBlocks);

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

                    default:
                        throw new Exception("Internal error. Unknown MIXXX type");
                }
            }
        }

        void ProcessText(MIText text,
            FileData fd,
            List<VariablesBlock> variableBlocks)
        {
            void ProcessHeader()
            {
                {
                    Regex r = new Regex("^[ \t]*Profile[ \t\n]*:[ \t\n]*([A-Za-z0-9\\-]+)");
                    Match m = r.Match(text.Line);
                    if (m.Success == true)
                    {
                        String profileName = m.Groups[1].Value;
                        StartNewProfile(profileName);
                        return;
                    }
                }
                {
                    Regex r = new Regex("^[ \t]*Extension[ \t\n]*:[ \t\n]*([A-Za-z0-9\\-]+)");
                    Match m = r.Match(text.Line);
                    if (m.Success == true)
                    {
                        String extensionName = m.Groups[1].Value;
                        StartNewExtension(extensionName);
                        return;
                    }
                }
                {
                    Regex r = new Regex("^[ \t]*Id[ \t\n]*:[ \t\n]*([A-Za-z0-9\\-]+)");
                    Match m = r.Match(text.Line);
                    if (m.Success == true)
                    {
                        String idName = m.Groups[1].Value;
                        this.profileVariables.Set("%Id%", idName);
                        return;
                    }
                }
            }

            String expandedText = variableBlocks.ReplaceText(text.Line);
            fd.Text.Append(expandedText);
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
                sb.Append($@"{applyStackFrame.SourceFile}#{applyStackFrame.LineNumber}\\n");
            }
            //Debug.Assert(sb.ToString().Contains("Configure.minc") == false);
            return sb.ToString();
        }
        
        void StartNewExtension(String extensionName)
        {
            // %Id% defaults to profile unless explicitly set (later)
            this.profileVariables.Set("%Id%", extensionName);

            String profileUrl = $"{this.BaseUrl}/StructureDefinition/{extensionName}";
            this.profileVariables.Set("%Url%", profileUrl);

            this.appliedMacros.Clear();
            this.incompatibleMacros.Clear();
        }

        void StartNewProfile(String profileName)
        {
            // %Id% defaults to profile unless explicitly set (later)
            this.profileVariables.Set("%Id%", profileName);

            String profileUrl = $"{this.BaseUrl}/StructureDefinition/{profileName}";
            this.profileVariables.Set("%Url%", profileUrl);

            this.appliedMacros.Clear();
            this.incompatibleMacros.Clear();
        }

        void ProcessApply(MIApply apply,
            FileData fd,
            List<VariablesBlock> variableBlocks)
        {
            const String fcn = "ProcessApply";

            List<VariablesBlock> local = new List<VariablesBlock>();
            local.AddRange(variableBlocks);

            if (this.Macros.TryGetMacro(apply.Usings, apply.Name, out MIMacro macro) == false)
            {
                String fullMsg = $"{apply.SourceFile}, line {apply.LineNumber} Macro {apply.Name} not found.";
                fullMsg += this.ApplyLongStackTrace();
                this.ConversionError(ClassName, fcn, fullMsg);
                return;
            }

            if (macro.Parameters.Count != apply.Parameters.Count)
            {
                String fullMsg = $"{apply.SourceFile}, line {apply.LineNumber} Macro {apply.Name} requires {macro.Parameters.Count} parameters, called with {apply.Parameters.Count}.";
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

            if ((macro.OnceFlag == true) && (firstFlag == false))
                return;

            if (this.incompatibleMacros.Contains(apply.Name))
            {
                String fullMsg = $"{apply.SourceFile}, line {apply.LineNumber} Macro {apply.Name} has been marked as incompatible with this profile";
                fullMsg += this.ApplyLongStackTrace();
                this.ConversionError(ClassName, fcn, fullMsg);
                return;
            }

                VariablesBlock vbParameters = new VariablesBlock();
                for (Int32 i = 0; i < apply.Parameters.Count; i++)
                {
                    String pName = macro.Parameters[i];
                    String pValue = apply.Parameters[i];
                    pValue = variableBlocks.ReplaceText(pValue);
                    vbParameters.Add(pName, pValue);
                }

                vbParameters.Add("%ApplySourceFile%", apply.SourceFile.Replace("\\", "/"));
                vbParameters.Add("%ApplyLineNumber%", apply.LineNumber.ToString());

                local.Insert(0, vbParameters);

            FileData macroData = fd;
            if (String.IsNullOrEmpty(macro.Redirect) == false)
                this.GetFileData(macro.Redirect, out macroData);

            this.applyStack.Push(apply);                    // this is for stack tracing during errors
            vbParameters.Add("%ApplyStackFrame%", this.ApplyShortStackTrace().Replace("\\", "/"));
            this.Process(macro.Items, macroData, local);
            this.applyStack.Pop();
        }

        void ProcessIncompatible(MIIncompatible incompatible,
            FileData fd,
            List<VariablesBlock> variableBlocks)
        {
            const String fcn = "ProcessIncompatible";

            if (this.Macros.TryGetMacro(this.usings, incompatible.Name, out MIMacro macro) == false)
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

                String dir = Path.GetDirectoryName(outputPath);
                if (Directory.Exists(dir) == false)
                    Directory.CreateDirectory(dir);

                if (FileTools.WriteModifiedText(outputPath, text) == true)
                    this.ConversionInfo(this.GetType().Name,
                        fcn,
                        $"Saving {Path.GetFileName(outputPath)}");

                // Mark file as updated. If file cleaning on, this will stop file
                // from being deleted during clean phase.
                this.fc.Mark(outputPath);
            }

            this.ConversionInfo(this.GetType().Name, fcn, $"Saving all processed files");
            foreach (FileData f in this.FileItems.Values)
            {
                String outputPath = Path.Combine(BaseOutputDir, f.RelativePath);
                string outText = f.Text.ToString();
                Save(outputPath, outText);
            }

            this.fc.DeleteUnMarkedFiles();
        }
    }
}
