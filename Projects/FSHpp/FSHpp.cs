using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Antlr4.Runtime.Tree;
using DevTools;

namespace FSHpp
{
    public class FSHpp : ConverterBase
    {
        private const String FSHSuffix = ".fsh";
        private const String FSHppSuffix = ".fshpp";

        public class FSHFile
        {
            public String FilePath;
            public NodeRule Doc;
        }

        public String[] InputLines { get; set; }

        public List<FSHFile> fshFiles = new List<FSHFile>();

        public Dictionary<string, NodeRule> AliasDict = new Dictionary<string, NodeRule>();
        public Dictionary<string, NodeRule> ProfileDict = new Dictionary<string, NodeRule>();
        public Dictionary<string, NodeRule> ExtensionDict = new Dictionary<string, NodeRule>();
        public Dictionary<string, NodeRule> InvariantDict = new Dictionary<string, NodeRule>();
        public Dictionary<string, NodeRule> InstanceDict = new Dictionary<string, NodeRule>();
        public Dictionary<string, NodeRule> ValueSetDict = new Dictionary<string, NodeRule>();
        public Dictionary<string, NodeRule> CodeSystemDict = new Dictionary<string, NodeRule>();
        public Dictionary<string, NodeRule> RuleSetDict = new Dictionary<string, NodeRule>();
        public Dictionary<string, NodeRule> MacroDict = new Dictionary<string, NodeRule>();
        public Dictionary<string, NodeRule> MappingDict = new Dictionary<string, NodeRule>();

        /// <summary>
        /// Parse input text.
        /// </summary>
        public FSHFile Parse(String fshText, String fileName)
        {
            fshText = fshText.Replace("\r", "");
            this.InputLines = fshText.Split('\n');

            FSHLexer lexer = new FSHLexer(new AntlrInputStream(fshText));

            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(new FSHErrorListenerLexer(this, fileName));

            FSHParser parser = new FSHParser(new CommonTokenStream(lexer));

            parser.RemoveErrorListeners();
            parser.AddErrorListener(new FSHErrorListenerParser(this, fileName));

            ParseTreeWalker walker = new ParseTreeWalker();
            FSHListener listener = new FSHListener(fshText);
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
        public void ProcessFile(String path)
        {
            const String fcn = "ProcessFile";

            this.ConversionInfo(this.GetType().Name, fcn, $"Processing file {path}");
            String fshText = File.ReadAllText(path);
            FSHFile f = this.Parse(fshText, Path.GetFileName(path));
            f.FilePath = path;
        }

        /// <summary>
        /// Process all files in indicated dir and sub dirs.
        /// </summary>
        public void ProcessDir(String path, String filter = FSHpp.FSHSuffix)
        {
            const String fcn = "ProcessDir";

            this.ConversionInfo(this.GetType().Name, fcn, $"Processing directory {path}, filter {filter}");
            foreach (String subDir in Directory.GetDirectories(path))
                ProcessDir(subDir, filter);

            foreach (String file in Directory.GetFiles(path, filter))
                ProcessFile(file);
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
        public void SaveAll()
        {
            const String fcn = "SaveAll";

            this.ConversionInfo(this.GetType().Name, fcn, $"Saving all processed files");
            foreach (FSHFile f in this.fshFiles)
            {
                String outputPath = Path.Combine(
                    Path.GetDirectoryName(f.FilePath),
                    $"{Path.GetFileNameWithoutExtension(f.FilePath)}.{FSHpp.FSHppSuffix}"
                );
                File.WriteAllText(outputPath, f.Doc.ToFSH());
            }
        }
    }
}
