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

namespace FSHpp
{
    public class FSHpp
    {
        private const String FSHSuffix = ".fsh";
        private const String FSHppSuffix = ".fshpp";

        public class FSHFile
        {
            public String FilePath;
            public NodeRule Doc;
        }

        public List<FSHFile> fshFiles = new List<FSHFile>();

        public Dictionary<string, NodeRule> aliasDict = new Dictionary<string, NodeRule>();
        public Dictionary<string, NodeRule> profileDict = new Dictionary<string, NodeRule>();
        public Dictionary<string, NodeRule> extensionDict = new Dictionary<string, NodeRule>();
        public Dictionary<string, NodeRule> invariantDict = new Dictionary<string, NodeRule>();
        public Dictionary<string, NodeRule> instanceDict = new Dictionary<string, NodeRule>();
        public Dictionary<string, NodeRule> valueSetDict = new Dictionary<string, NodeRule>();
        public Dictionary<string, NodeRule> codeSystemDict = new Dictionary<string, NodeRule>();
        public Dictionary<string, NodeRule> ruleSetDict = new Dictionary<string, NodeRule>();
        public Dictionary<string, NodeRule> mappingDict = new Dictionary<string, NodeRule>();

        /// <summary>
        /// Parse input text.
        /// </summary>
        public NodeRule Parse(String fshText)
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
            CollateFile(d);
            return d;
        }

        void CollateFile(NodeRule d)
        {
            foreach (NodeRule entity in d.ChildNodes.Rules())
            {
                if (entity.ChildNodes.Count != 1)
                    throw new Exception($"Invalid child nodes in entity record");
                NodeRule rule = (NodeRule)entity.ChildNodes.First();
                String entityName = rule.Name;
                switch (rule.NodeType.ToLower())
                {
                    case "alias":
                        aliasDict.Add(entityName, rule);
                        break;

                    case "profile":
                        profileDict.Add(entityName, rule);
                        break;

                    case "extension":
                        extensionDict.Add(entityName, rule);
                        break;

                    case "invariant":
                        invariantDict.Add(entityName, rule);
                        break;

                    case "instance":
                        instanceDict.Add(entityName, rule);
                        break;

                    case "valueset":
                        valueSetDict.Add(entityName, rule);
                        break;

                    case "codesystem":
                        codeSystemDict.Add(entityName, rule);
                        break;

                    case "ruleset":
                        ruleSetDict.Add(entityName, rule);
                        break;

                    case "mapping":
                        mappingDict.Add(entityName, rule);
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        /// <summary>
        /// Process single file.
        /// </summary>
        public void ProcessFile(String path)
        {
            String fshText = File.ReadAllText(path);
            NodeRule d = this.Parse(fshText);
            FSHFile f = new FSHFile
            {
                FilePath = path,
                Doc = d
            };
            this.fshFiles.Add(f);
        }

        /// <summary>
        /// Process all files in indicated dir and sub dirs.
        /// </summary>
        public void ProcessDir(String path, String filter = FSHpp.FSHSuffix)
        {
            foreach (String subDir in Directory.GetDirectories(path))
                ProcessDir(subDir, filter);

            foreach (String file in Directory.GetFiles(path, filter))
                ProcessFile(file);
        }

        /// <summary>
        /// Write all files back to disk.
        /// </summary>
        public void SaveAll()
        {
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
