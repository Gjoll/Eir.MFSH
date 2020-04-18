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
        public List<String> IncludeDirs;

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
            const bool DebugFlag = true;

            if (this.sources.Contains(sourceName))
                throw new Exception($"File {sourceName} has already been processed. Recursive include look?");
            this.sources.Add(sourceName);

            fshText = fshText.Replace("\r", "");
            String[] inputLines = fshText.Split('\n');

            MFSHLexerLocal lexer = new MFSHLexerLocal(new AntlrInputStream(fshText));
            lexer.DebugFlag = DebugFlag;
            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(new MFSHErrorListenerLexer(this, sourceName, inputLines));

            MFSHParserLocal parser = new MFSHParserLocal(new CommonTokenStream(lexer));
            parser.DebugFlag = DebugFlag;
            parser.Trace = false;
            parser.RemoveErrorListeners();
            parser.AddErrorListener(new MFSHErrorListenerParser(this, sourceName, inputLines));
            //parser.ErrorHandler = new BailErrorStrategy();

            MFSHVisitor visitor = new MFSHVisitor(this, sourceName);
            visitor.DebugFlag = DebugFlag;
            visitor.Visit(parser.document());

            fshText = fshText.Replace("\r", "");
            visitor.InputLines = fshText.Split('\n');

            return visitor.ParsedText.ToString();
        }

        /// <summary>
        /// Process single file.
        /// </summary>
        public void AddFile(String basePath, String path)
        {
            const String fcn = "ProcessFile";

            this.ConversionInfo(this.GetType().Name, fcn, $"Processing file {path}");
            String fshText = File.ReadAllText(path);
            FSHFile f = new FSHFile
            {
                Text = this.Parse(fshText, Path.GetFileName(path)),
            };

            if (path.StartsWith(basePath) == false)
                throw new Exception("Internal error. Path does not start with correct base path");
            String relativePath = path.Substring(basePath.Length);
            if (relativePath.StartsWith("\\"))
                relativePath = relativePath.Substring(1);
            f.RelativePath = relativePath;
            fshFiles.Add(f);
        }

        /// <summary>
        /// Process all files in indicated dir and sub dirs.
        /// </summary>
        public void ProcessDir(String path, String filter = MFsh.MFSHSuffix)
        {
            path = Path.GetFullPath(path);
            this.ProcessDir(path, path, filter);
        }

        void ProcessDir(String basePath,
            String path,
            String filter = MFsh.MFSHSuffix)
        {
            const String fcn = "AddDir";

            this.ConversionInfo(this.GetType().Name, fcn, $"Processing directory {path}, filter {filter}");
            foreach (String subDir in Directory.GetDirectories(path))
                this.ProcessDir(basePath, subDir, filter);

            foreach (String file in Directory.GetFiles(path, $"*{MFsh.MFSHSuffix}"))
                this.AddFile(basePath, file);
        }

        /// <summary>
        /// Write all files back to disk.
        /// </summary>
        public void SaveAll(String outputDir)
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

            outputDir = Path.GetFullPath(outputDir);
            this.ConversionInfo(this.GetType().Name, fcn, $"Saving all processed files");
            foreach (FSHFile f in this.fshFiles)
            {
                String outputPath = Path.Combine(outputDir, f.RelativePath);
                String dir = Path.GetDirectoryName(outputPath);
                outputPath = Path.Combine(dir,
                    $"{Path.GetFileNameWithoutExtension(outputPath)}{MFsh.FSHSuffix}"
                );
                Save(outputPath, f.Text);
            }
        }
    }
}
