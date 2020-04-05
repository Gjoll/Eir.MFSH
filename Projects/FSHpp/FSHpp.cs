﻿using Antlr4.Runtime;
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
        public void ProcessInput(NodeDocument d,
            String fshText)
        {
            fshText = fshText.Replace("\r", "");
            FSHLexer lexer = new FSHLexer(new AntlrInputStream(fshText));

            //lexer.RemoveErrorListeners();
            //lexer.AddErrorListener(new ThrowingErrorListener());

            FSHParser parser = new FSHParser(new CommonTokenStream(lexer));

            //parser.RemoveErrorListeners();
            //parser.AddErrorListener(new ThrowingErrorListener());

            VisitorInfo info = new VisitorInfo
            {
                Input = fshText
            };

            FSHVisitor fsh = new FSHVisitor(info);
            fsh.VisitDoc(parser.doc());
            info.CopyToEnd();
            //return info.Output.ToString();
        }

        public void ProcessFile(String path)
        {
            NodeDocument d = new NodeDocument
            {
                FileName = path
            };

            String fshText = File.ReadAllText(path);
            ProcessInput(d, fshText);
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
