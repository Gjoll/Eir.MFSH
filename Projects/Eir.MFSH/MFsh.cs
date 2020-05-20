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

namespace Eir.MFSH
{
    public class MFsh : ConverterBase
    {
        public bool DebugFlag { get; set; } = false;
        public MFshManager Mgr { get; }

        HashSet<String> appliedMacros = new HashSet<string>();
        HashSet<String> incompatibleMacros = new HashSet<string>();

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

        public String BaseUrl { get; set; }

        public VariablesBlock GlobalVars = new VariablesBlock();
        public Dictionary<String, FileData> FileItems =
            new Dictionary<String, FileData>();

        public MFsh()
        {
            this.Mgr = new MFshManager(this);
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

            String outputPath = Path.Combine(this.baseOutputDir, relativePath);
            this.Mgr.ParseOne(mfshText, Path.GetFileName(path), outputPath);

            //String baseRPath = path.Substring(BaseInputDir.Length);
            //if (baseRPath.StartsWith("\\"))
            //    baseRPath = baseRPath.Substring(1);
            //Int32 extensionIndex = baseRPath.LastIndexOf('.');
            //if (extensionIndex > 0)
            //    baseRPath = baseRPath.Substring(0, extensionIndex);

            //String baseName = Path.GetFileName(baseRPath);
            //String baseDir = Path.GetDirectoryName(baseRPath);

            //this.GlobalVars.Set("%BasePath%", baseRPath);
            //this.GlobalVars.Set("%BaseDir%", baseDir);
            //this.GlobalVars.Set("%BaseName%", baseName);
            //this.GlobalVars.Set("%SavePath%", $"{baseRPath}{MFsh.FSHSuffix}");

            //StackFrame frame = this.Parse(fshText,
            //    baseName,
            //    Path.GetDirectoryName(path));

            //void SaveData(FileData fileData)
            //{
            //    fileData.ProcessVariables(this.GlobalVars);
            //    if (this.FileItems.TryGetValue(fileData.RelativePath, out FileData existingReDir) == true)
            //        existingReDir.AppendText(fileData.Text());
            //    else
            //        this.FileItems.Add(fileData.RelativePath, fileData);
            //}

            //SaveData(frame.Data);
            //foreach (FileData reDir in frame.Redirections)
            //    SaveData(reDir);
        }

        void LoadDir(String path,
            String filter = MFsh.MFSHSuffix)
        {
            const String fcn = "AddDir";

            if (String.IsNullOrEmpty(this.BaseInputDir) == true)
                throw new Exception($"BaseInputDir not set");

            this.ConversionInfo(this.GetType().Name, fcn, $"Loading directory {path}, filter {filter}");
            foreach (String subDir in Directory.GetDirectories(path))
                this.LoadDir(subDir, filter);

            foreach (String file in Directory.GetFiles(path, $"*{MFsh.MFSHSuffix}"))
                this.LoadFile(file);
        }

        public void Load(String path,
            String filter = MFsh.MFSHSuffix)
        {
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
                this.ConversionError("mfsh", "Load", fullMsg);
            }
        }

        /// <summary>
        /// Process all files in indicated dir and sub dirs.
        /// </summary>
        public void Process()
        {
            if (String.IsNullOrEmpty(this.BaseUrl) == true)
                throw new Exception($"BaseUrl not set");
            foreach (MIPreFsh fsh in this.Mgr.Fsh)
                fsh.Items = this.Process(fsh.Items, this.GlobalVars);
        }

        List<MIBase> Process(List<MIBase> inputItems,
            VariablesBlock variablesBlock)
        {
            List<MIBase> outputItems = new List<MIBase>();

            Int32 i = 0;
            while (i < inputItems.Count)
            {
                MIBase b = inputItems[i];
                switch (b)
                {
                    case MIText text:
                        this.ProcessText(text, outputItems, variablesBlock);
                        i += 1;
                        break;

                    case MIApply apply:
                        this.ProcessApply(apply, outputItems, variablesBlock);
                        i += 1;
                        break;

                    case MIIncompatible incompatible:
                        this.ProcessIncompatible(incompatible, outputItems, variablesBlock);
                        i += 1;
                        break;

                    default:
                        throw new Exception("Internal error. Unknown MIXXX type");
                }
            }
            return outputItems;
        }

        void ProcessText(MIText text,
            List<MIBase> outputItems,
            VariablesBlock variablesBlock)
        {
            String expandedText = variablesBlock.ReplaceText(text.Line);
            outputItems.Add(new MIText(text.SourceFile, text.LineNumber)
            {
                Line = expandedText
            });

            Regex r = new Regex("^Profile[ \n]*:[ \n]*([A-Za-z0-9]+)");
            Match m = r.Match(text.Line);
            if (m.Success == true)
            {
                String profileName = m.Groups[1].Value;
                StartNewProfile(profileName);
                this.GlobalVars.Set("%Profile%", profileName);
            }
        }

        void StartNewProfile(String profileName)
        {
            this.GlobalVars.Set("%Profile%", profileName);
            this.appliedMacros.Clear();
            this.incompatibleMacros.Clear();
        }

        void ProcessApply(MIApply apply,
            List<MIBase> outputItems,
            VariablesBlock variablesBlock)
        {
            if (this.Mgr.TryGetMacro(apply.Name, out MIMacro macro) == false)
            {
                String fullMsg = $"{apply.SourceFile}, line {apply.LineNumber} Macro {apply.Name} not found.";
                this.ConversionError("mfsh", "ProcessApply", fullMsg);
                return;
            }

            if (macro.Parameters.Count != apply.Parameters.Count)
            {
                String fullMsg = $"{apply.SourceFile}, line {apply.LineNumber} Macro {apply.Name} requires {macro.Parameters.Count} parameters, called with {apply.Parameters.Count}.";
                this.ConversionError("mfsh", "ProcessApply", fullMsg);
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
                this.ConversionError("mfsh", "ProcessApply", fullMsg);
                return;
            }

            VariablesBlock vbParameters = new VariablesBlock();
            for (Int32 i = 0; i < apply.Parameters.Count; i++)
            {
                String pName = macro.Parameters[i];
                String pValue = apply.Parameters[i];
                pValue = variablesBlock.ReplaceText(pValue);
                vbParameters.Add(pName, pValue);
            }

            // Replace parameters with parameter value.
            List<MIBase> macroItems = this.Process(macro.Items, vbParameters);
            // Replace global variables with global variable value.
            macroItems = this.Process(macroItems, variablesBlock);
            // Add expanded items to output list.
            outputItems.AddRange(macroItems);
        }

        void ProcessIncompatible(MIIncompatible incompatible,
            List<MIBase> outputItems,
            VariablesBlock variablesBlock)
        {
            if (this.incompatibleMacros.Contains(incompatible.Name) == true)
                return;
            if (this.appliedMacros.Contains(incompatible.Name) == true)
            {
                String fullMsg = $"{incompatible.SourceFile}, line {incompatible.LineNumber} Macro {incompatible.Name} is incompatible and has already been applied.";
                this.ConversionError("mfsh", "ProcessApply", fullMsg);
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
                {
                    if (File.Exists(outputPath))
                        File.Delete(outputPath);
                    return;
                }

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
                String outputPath = Path.Combine(BaseOutputDir, f.RelativePath);
                string outText = f.SaveText();
                Save(outputPath, outText);
            }
        }
    }
}
