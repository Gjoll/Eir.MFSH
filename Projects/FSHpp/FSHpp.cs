using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public List<FSHFile> fshFiles = new List<FSHFile>();

        public Dictionary<string, NodeRule> AliasDict = new Dictionary<string, NodeRule>();
        public Dictionary<string, NodeRule> ProfileDict = new Dictionary<string, NodeRule>();
        public Dictionary<string, NodeRule> ExtensionDict = new Dictionary<string, NodeRule>();
        public Dictionary<string, NodeRule> InvariantDict = new Dictionary<string, NodeRule>();
        public Dictionary<string, NodeRule> InstanceDict = new Dictionary<string, NodeRule>();
        public Dictionary<string, NodeRule> ValueSetDict = new Dictionary<string, NodeRule>();
        public Dictionary<string, NodeRule> CodeSystemDict = new Dictionary<string, NodeRule>();
        public Dictionary<string, NodeRule> RuleSetDict = new Dictionary<string, NodeRule>();
        public Dictionary<string, NodeRule> MappingDict = new Dictionary<string, NodeRule>();

        /// <summary>
        /// Parse input text.
        /// </summary>
        public FSHFile Parse(String fshText)
        {
            fshText = fshText.Replace("\r", "");
            FSHLexer lexer = new FSHLexer(new AntlrInputStream(fshText));

            //lexer.RemoveErrorListeners();
            //lexer.AddErrorListener(new ThrowingErrorListener());

            FSHParser parser = new FSHParser(new CommonTokenStream(lexer));

            //parser.RemoveErrorListeners();
            //parser.AddErrorListener(new ThrowingErrorListener());

            //FSHVisitor fsh = new FSHVisitor(fshText);
            //return (NodeDocument) fsh.VisitDoc(parser.doc());

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
            FSHFile f = this.Parse(fshText);
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

        public void Process()
        {
            new Processors.Collator(this).Process();
            new Processors.Macros(this).Process();
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
