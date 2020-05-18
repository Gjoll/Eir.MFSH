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

namespace MFSH.Parser2
{
    public class MFshManager
    {
        public bool DebugFlag { get; set; } = true;

        public List<String> Paths = new List<string>();

        public String BaseUrl { get; set; }
        private MFsh mfsh;

        public MFshManager(MFsh mfsh)
        {
            this.mfsh = mfsh;
        }

        public ParseBlock ParseOne(String fshText, String sourceName, String outputPath)
        {
            fshText = fshText.Replace("\r", "");
            String[] inputLines = fshText.Split('\n');

            Parser2.MFSHLexerLocal lexer = new Parser2.MFSHLexerLocal(new AntlrInputStream(fshText));
            lexer.DebugFlag = DebugFlag;
            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(new MFSHErrorListenerLexer(this.mfsh,
                "MFsh Lexer",
                sourceName,
                inputLines));

            Parser2.MFSHParserLocal parser = new Parser2.MFSHParserLocal(new CommonTokenStream(lexer));
            parser.DebugFlag = DebugFlag;
            parser.Trace = false;
            parser.RemoveErrorListeners();
            parser.AddErrorListener(new MFSHErrorListenerParser(this.mfsh,
                "MFsh Parser",
                sourceName,
                inputLines));
            //parser.ErrorHandler = new BailErrorStrategy();

            Parser2.MFSHVisitor visitor = new Parser2.MFSHVisitor(this.mfsh, sourceName);
            visitor.DebugFlag = DebugFlag;
            visitor.Visit(parser.document());
            if (visitor.state.Count != 1)
            {
                String fullMsg = $"Error processing {sourceName}. Unterminated #{{Command}}";
                this.mfsh.ConversionError("mfsh", "ProcessInclude", fullMsg);
            }

            ParseBlock block = visitor.Current;
            block.OutputPath = outputPath;
            return block;
        }
    }
}
