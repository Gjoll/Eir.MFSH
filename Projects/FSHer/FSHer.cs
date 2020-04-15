using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Antlr4.Runtime.Tree;
using Eir.DevTools;
using Eir.FSHer.Antlr;

namespace Eir.FSHer
{
    public class FSHer : ConverterBase
    {
        private const String FSHSuffix = ".fsh";
        private const String FSHerSuffix = ".fsher";

        public String[] InputLines { get; set; }

        public List<FSHFile> fshFiles = new List<FSHFile>();

        public Dictionary<string, NodeRule> ProfileDict = new Dictionary<string, NodeRule>();
        public Dictionary<string, NodeRule> ExtensionDict = new Dictionary<string, NodeRule>();
        public Dictionary<string, NodeRule> InvariantDict = new Dictionary<string, NodeRule>();
        public Dictionary<string, NodeRule> InstanceDict = new Dictionary<string, NodeRule>();
        public Dictionary<string, NodeRule> ValueSetDict = new Dictionary<string, NodeRule>();
        public Dictionary<string, NodeRule> CodeSystemDict = new Dictionary<string, NodeRule>();
        public Dictionary<string, NodeRule> RuleSetDict = new Dictionary<string, NodeRule>();
        public Dictionary<string, NodeRule> MacroDict = new Dictionary<string, NodeRule>();
        public Dictionary<string, NodeRule> MappingDict = new Dictionary<string, NodeRule>();

        public IEnumerable<IEnumerable<NodeRule>> AllGroups()
        {
            yield return ProfileDict.Values;
            yield return ExtensionDict.Values;
            yield return InvariantDict.Values;
            yield return InstanceDict.Values;
            yield return ValueSetDict.Values;
            yield return CodeSystemDict.Values;
            yield return RuleSetDict.Values;
            yield return MacroDict.Values;
            yield return MappingDict.Values;
        }

        public IEnumerable<NodeRule> AllItems()
        {
            foreach (IEnumerable<NodeRule> group in AllGroups())
            {
                foreach (NodeRule item in group)
                    yield return item;
            }
        }

        /// <summary>
        /// Parse input text.
        /// </summary>
        public FSHFile Parse(String fshText, String fileName)
        {
            fshText = fshText.Replace("\r", "");
            this.InputLines = fshText.Split('\n');

            FSHLexer lexer = new FSHLexerLocal(new AntlrInputStream(fshText));

            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(new FSHErrorListenerLexer(this, fileName));

            FSHParser parser = new FSHParserLocal(new CommonTokenStream(lexer));
            parser.Trace = false;

            parser.RemoveErrorListeners();
            parser.AddErrorListener(new FSHErrorListenerParser(this, fileName));

            ParseTreeWalker walker = new ParseTreeWalker();
            FSHListener listener = new FSHListener(fshText, fileName);
            walker.Walk(listener, parser.doc());
            NodeRule d = (NodeRule)listener.Head.ChildNodes.First();

            FSHFile f = new FSHFile
            {
                Doc = d
            };
            this.fshFiles.Add(f);
            return f;
        }

        /// <summary>
        /// Process single file.
        /// </summary>
        public void AddFile(String basePath, String path)
        {
            const String fcn = "ProcessFile";

            this.ConversionInfo(this.GetType().Name, fcn, $"Processing file {path}");
            String fshText = File.ReadAllText(path);
            FSHFile f = this.Parse(fshText, Path.GetFileName(path));
            if (path.StartsWith(basePath) == false)
                throw new Exception("Internal error. Path does not start with correct base path");
            String relativePath = path.Substring(basePath.Length);
            if (relativePath.StartsWith("\\"))
                relativePath = relativePath.Substring(1);
            f.RelativePath = relativePath;
        }

        /// <summary>
        /// Process all files in indicated dir and sub dirs.
        /// </summary>
        public void AddDir(String path, String filter = FSHer.FSHerSuffix)
        {
            path = Path.GetFullPath(path);
            AddDir(path, path, filter);
        }


        void AddDir(String basePath,
            String path,
            String filter = FSHer.FSHerSuffix)
        {
            const String fcn = "ProcessDir";

            this.ConversionInfo(this.GetType().Name, fcn, $"Processing directory {path}, filter {filter}");
            foreach (String subDir in Directory.GetDirectories(path))
                this.AddDir(basePath, subDir, filter);

            foreach (String file in Directory.GetFiles(path, $"*{FSHer.FSHerSuffix}"))
                this.AddFile(basePath, file);
        }

        public bool Process()
        {
            if (this.HasErrors == true)
                return false;

            new Processors.Collator(this).Process();
            new Processors.Macros(this).Process();

            return this.HasErrors == false;
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
                    $"{Path.GetFileNameWithoutExtension(outputPath)}{FSHer.FSHSuffix}"
                );
                Save(outputPath, f.Doc.ToFSH());
            }
        }
    }
}
