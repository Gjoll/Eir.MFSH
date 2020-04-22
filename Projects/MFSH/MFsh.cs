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
using MFSH;

namespace MFSH
{
    public class MFsh : ConverterBase
    {
        public bool DebugFlag { get; set; } = false;
        public List<String> IncludeDirs;

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

        public Dictionary<String, DefineInfo> Defines = new Dictionary<string, DefineInfo>();
        public HashSet<String> Includes = new HashSet<string>();
        // Keep track of include files so we dont end up in recursive loop.
        List<String> sources = new List<string>();

        private const String FSHSuffix = ".fsh";
        private const String MFSHSuffix = ".mfsh";
        private List<FSHFile> fshFiles = new List<FSHFile>();

        public MFsh()
        {
            this.IncludeDirs = new List<string>();
            this.IncludeDirs.Add(".");
        }
        public string Parse(String fshText, String sourceName)
        {
            this.Includes.Clear();
            this.Defines.Clear();
            this.sources.Clear();

            return SubParse(fshText, sourceName);
        }


        /// <summary>
        /// Parse input text.
        /// </summary>
        public string SubParse(String fshText, String sourceName)
        {
            if (this.sources.Contains(sourceName))
                throw new Exception($"File {sourceName} has already been processed. Recursive include look?");
            this.sources.Add(sourceName);

            string text = PreParseText(fshText, sourceName);
            text = ParseText(text, sourceName);
            return text;
        }

        public string PreParseText(String fshText, String sourceName)
        {
            if (fshText.EndsWith("\n") == false)
               fshText = fshText + "\n";

            fshText = fshText.Replace("\r", "");
            String[] inputLines = fshText.Split('\n');

            PreParser.MFSHPreLexerLocal lexer = new PreParser.MFSHPreLexerLocal(new AntlrInputStream(fshText));
            lexer.DebugFlag = DebugFlag;
            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(new MFSHErrorListenerLexer(this, sourceName, inputLines));

            PreParser.MFSHPreParserLocal parser = new PreParser.MFSHPreParserLocal(new CommonTokenStream(lexer));
            parser.DebugFlag = DebugFlag;
            parser.Trace = false;
            parser.RemoveErrorListeners();
            parser.AddErrorListener(new MFSHErrorListenerParser(this, sourceName, inputLines));

            PreParser.MFSHPreVisitor visitor = new PreParser.MFSHPreVisitor(sourceName);
            visitor.DebugFlag = DebugFlag;
            visitor.Visit(parser.text());
            return visitor.ParsedText.ToString();
        }

        public string ParseText(String fshText, String sourceName)
        {
            fshText = fshText.Replace("\r", "");
            String[] inputLines = fshText.Split('\n');

            Parser.MFSHLexerLocal lexer = new Parser.MFSHLexerLocal(new AntlrInputStream(fshText));
            lexer.DebugFlag = DebugFlag;
            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(new MFSHErrorListenerLexer(this, sourceName, inputLines));

            Parser.MFSHParserLocal parser = new Parser.MFSHParserLocal(new CommonTokenStream(lexer));
            parser.DebugFlag = DebugFlag;
            parser.Trace = false;
            parser.RemoveErrorListeners();
            parser.AddErrorListener(new MFSHErrorListenerParser(this, sourceName, inputLines));
            //parser.ErrorHandler = new BailErrorStrategy();

            Parser.MFSHVisitor visitor = new Parser.MFSHVisitor(this, sourceName);
            visitor.DebugFlag = DebugFlag;
            visitor.Visit(parser.document());
            if (visitor.state.Count != 1)
            {
                String fullMsg = $"Error processing {sourceName}. Unterminated #{{Command}}";
                this.ConversionError("mfsh", "ProcessInclude", fullMsg);
            }

            return visitor.ParsedText.ToString();
        }

        /// <summary>
        /// Process single file.
        /// </summary>
        public void ProcessFile(String path)
        {
            const String fcn = "ProcessFile";

            path = Path.GetFullPath(path);
            this.ConversionInfo(this.GetType().Name, fcn, $"Processing file {path}");
            String fshText = File.ReadAllText(path);
            FSHFile f = new FSHFile
            {
                Text = this.Parse(fshText, Path.GetFileName(path)),
            };

            if (path.StartsWith(BaseInputDir) == false)
                throw new Exception("Internal error. Path does not start with correct base path");
            String relativePath = path.Substring(BaseInputDir.Length);
            if (relativePath.StartsWith("\\"))
                relativePath = relativePath.Substring(1);
            f.RelativePath = relativePath;
            fshFiles.Add(f);
        }

        /// <summary>
        /// Process all files in indicated dir and sub dirs.
        /// </summary>
        public void Process()
        {
            foreach (String path in this.Paths)
            {
                if (Directory.Exists(path))
                    this.ProcessDir(path);
                else if (File.Exists(path))
                    this.ProcessFile(path);
                else
                    throw new Exception($"Path {path} does not exist");
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

            void Save(String outputPath,
                String text)
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
            foreach (FSHFile f in this.fshFiles)
            {
                String outputPath = Path.Combine(BaseOutputDir, f.RelativePath);
                String dir = Path.GetDirectoryName(outputPath);
                outputPath = Path.Combine(dir,
                    $"{Path.GetFileNameWithoutExtension(outputPath)}{MFsh.FSHSuffix}"
                );
                Save(outputPath, f.Text);
            }
        }
    }
}
