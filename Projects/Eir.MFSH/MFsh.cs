using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Antlr4.Runtime.Tree;
using Eir.DevTools;
using Eir.MFSH;

namespace Eir.MFSH
{
    public class MFsh : ConverterBase
    {
        public bool DebugFlag { get; set; } = true;
        public List<String> IncludeDirs;

        // dir containing file being parsed. Usd for local includes.
        public String LocalDir { get; set; }
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

        public List<String> Paths = new List<string>();

        public Dictionary<String, MacroDefinition> Defines = new Dictionary<string, MacroDefinition>();
        public HashSet<String> Includes = new HashSet<string>();
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
            this.IncludeDirs = new List<string>();
            this.IncludeDirs.Add(".");
        }

        // Trims out 'once' macros that should not be expanded.
        void ProcessMacroText(StackFrame frame)
        {
            bool eolnFlag = false;
            StringBuilder sb = new StringBuilder();
            String text = frame.Data.Text().Replace("\r", "");
            bool skippingFlag = false;
            Int32 applyStartCount = 0;

            void AppendLine(String text)
            {
                if (skippingFlag == false)
                {
                    if (eolnFlag == true)
                        sb.Append("\n");
                    else
                        eolnFlag = true;
                    sb.Append(text);
                }
            }

            void ProcessLine(String line)
            {
                if (line.StartsWith("#apply once "))
                {
                    eolnFlag = false;
                    String macroName = line.Substring(12);
                    if (frame.AppliedMacros.TryGetValue(macroName, out ApplyInfo appliedMacro) == false)
                        throw new Exception($"Embedded Macro {macroName} not found");
                    if (appliedMacro.AppliedFlag == true)
                        skippingFlag = true;
                    appliedMacro.AppliedFlag = true;
                    return;
                }

                if (line.StartsWith("#end"))
                {
                    eolnFlag = false;
                    if (applyStartCount == 0)
                        skippingFlag = false;
                    else
                        applyStartCount -= 1;
                    return;
                }

                AppendLine(line);
            }

            foreach (String line in text.Split("\n"))
                ProcessLine(line);
            sb.Append("\n");
            frame.Data.SetText(sb.ToString());
        }

        public StackFrame Parse(String fshText,
            String sourceName,
            String localDir)
        {
            this.Includes.Clear();
            this.Defines.Clear();
            this.sources.Clear();

            StackFrame frame =  SubParse(fshText, sourceName, localDir);
            ProcessMacroText(frame);
            return frame;
        }


        /// <summary>
        /// Parse input text.
        /// </summary>
        public StackFrame SubParse(String text,
            String sourceName,
            String localDir)
        {
            String saveLocalDir = this.LocalDir;
            this.LocalDir = localDir;
            if (this.sources.Contains(sourceName))
                throw new Exception($"File {sourceName} has already been processed. Recursive include look?");
            this.sources.Add(sourceName);

            StackFrame retVal = ParseText(text, sourceName);
            this.LocalDir = saveLocalDir;
            return retVal;
        }

        public string PreParseText(String fshText, String sourceName)
        {
            throw new NotImplementedException();
        }

        public StackFrame ParseText(String fshText, String sourceName)
        {
            //fshText = fshText.Replace("\r", "");
            //String[] inputLines = fshText.Split('\n');

            //Parser2.MFSHLexerLocal lexer = new Parser2.MFSHLexerLocal(new AntlrInputStream(fshText));
            //lexer.DebugFlag = DebugFlag;
            //lexer.RemoveErrorListeners();
            //lexer.AddErrorListener(new MFSHErrorListenerLexer(this,
            //    "MFsh Lexer",
            //    sourceName,
            //    inputLines));

            //Parser2.MFSHParserLocal parser = new Parser2.MFSHParserLocal(new CommonTokenStream(lexer));
            //parser.DebugFlag = DebugFlag;
            //parser.Trace = false;
            //parser.RemoveErrorListeners();
            //parser.AddErrorListener(new MFSHErrorListenerParser(this,
            //    "MFsh Parser",
            //    sourceName,
            //    inputLines));
            ////parser.ErrorHandler = new BailErrorStrategy();

            //Parser2.MFSHVisitor visitor = new Parser2.MFSHVisitor(this, sourceName);
            //visitor.DebugFlag = DebugFlag;
            //visitor.Visit(parser.document());
            //if (visitor.state.Count != 1)
            //{
            //    String fullMsg = $"Error processing {sourceName}. Unterminated #{{Command}}";
            //    this.ConversionError("mfsh", "ProcessInclude", fullMsg);
            //}
            //return visitor.Current;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Process single file.
        /// </summary>
        public void ProcessFile(String path)
        {
            const String fcn = "ProcessFile";

            path = Path.GetFullPath(path);
            if (path.StartsWith(BaseInputDir) == false)
                throw new Exception("Internal error. Path does not start with correct base path");

            this.ConversionInfo(this.GetType().Name, fcn, $"Processing file {path}");
            String fshText = File.ReadAllText(path);

            String baseRPath = path.Substring(BaseInputDir.Length);
            if (baseRPath.StartsWith("\\"))
                baseRPath = baseRPath.Substring(1);
            Int32 extensionIndex = baseRPath.LastIndexOf('.');
            if (extensionIndex > 0)
                baseRPath = baseRPath.Substring(0, extensionIndex);

            String baseName = Path.GetFileName(baseRPath);
            String baseDir = Path.GetDirectoryName(baseRPath);

            this.GlobalVars.Set("%BasePath%", baseRPath);
            this.GlobalVars.Set("%BaseDir%", baseDir);
            this.GlobalVars.Set("%BaseName%", baseName);
            this.GlobalVars.Set("%SavePath%", $"{baseRPath}{MFsh.FSHSuffix}");

            StackFrame frame = this.Parse(fshText,
                baseName,
                Path.GetDirectoryName(path));

            void SaveData(FileData fileData)
            {
                fileData.ProcessVariables(this.GlobalVars);
                if (this.FileItems.TryGetValue(fileData.RelativePath, out FileData existingReDir) == true)
                    existingReDir.AppendText(fileData.Text());
                else
                    this.FileItems.Add(fileData.RelativePath, fileData);
            }

            SaveData(frame.Data);
            foreach (FileData reDir in frame.Redirections)
                SaveData(reDir);
        }

        /// <summary>
        /// Process all files in indicated dir and sub dirs.
        /// </summary>
        public void Process()
        {
            if (String.IsNullOrEmpty(this.BaseUrl) == true)
                throw new Exception($"BaseUrl not set");

            foreach (String path in this.Paths)
            {
                try
                {
                    if (Directory.Exists(path))
                        this.ProcessDir(path);
                    else if (File.Exists(path))
                        this.ProcessFile(path);
                    else
                        throw new Exception($"Path {path} does not exist");
                }
                catch (Exception err)
                {
                    String fullMsg = $"Error processing {Path.GetFileName(path)}. '{err.Message}'";
                    this.ConversionError("mfsh", "ProcessInclude", fullMsg);
                }
            }
        }

        void ProcessDir(String path,
            String filter = MFsh.MFSHSuffix)
        {
            const String fcn = "AddDir";

            this.ConversionInfo(this.GetType().Name, fcn, $"Processing directory {path}, filter {filter}");
            foreach (String subDir in Directory.GetDirectories(path))
                this.ProcessDir(subDir, filter);

            foreach (String file in Directory.GetFiles(path, $"*{MFsh.MFSHSuffix}"))
                this.ProcessFile(file);
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

            this.BaseOutputDir = Path.GetFullPath(BaseOutputDir);
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
