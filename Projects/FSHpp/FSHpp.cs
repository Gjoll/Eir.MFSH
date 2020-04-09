using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Antlr4.Runtime.Tree;

namespace FSHpp
{
    public class FSHpp
    {
        public NodeDocument ProcessInput(String fshText)
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
            return listener.Doc;
        }

        public void ProcessFile(String path)
        {
            String fshText = File.ReadAllText(path);
            NodeDocument d = ProcessInput(fshText);
            d.FileName = path;
        }

        public void ProcessDir(String path, String filter)
        {
            foreach (String subDir in Directory.GetDirectories(path))
                ProcessDir(subDir, filter);

            foreach (String file in Directory.GetFiles(path, filter))
                ProcessFile(file);
        }
    }

}
