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
using Eir.MFSH.Parser;

namespace Eir.MFSH
{
    public class MFshManager
    {
        Dictionary<String, MIMacro> Macros = new Dictionary<string, MIMacro>();
        public List<MIPreFsh> Fsh = new List<MIPreFsh>();

        public bool TryGetMacro(String name, out MIMacro block) => this.Macros.TryGetValue(name, out block);
        public bool TryAddMacro(String name, MIMacro macro) => this.Macros.TryAdd(name, macro);

        public bool DebugFlag => this.Mfsh.DebugFlag;

        public List<String> Paths = new List<string>();

        public String BaseUrl { get; set; }
        public MFsh Mfsh { get; }

        public MFshManager(MFsh mfsh)
        {
            this.Mfsh = mfsh;
        }

        public MIPreFsh ParseOne(String fshText,
            String sourceName,
            String relativePath)
        {
            fshText = fshText.Replace("\r", "");
            String[] inputLines = fshText.Split('\n');

            Parser.MFSHLexerLocal lexer = new Parser.MFSHLexerLocal(new AntlrInputStream(fshText));
            lexer.DebugFlag = DebugFlag;
            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(new MFSHErrorListenerLexer(this.Mfsh,
                "MFsh Lexer",
                sourceName,
                inputLines));

            Parser.MFSHParserLocal parser = new Parser.MFSHParserLocal(new CommonTokenStream(lexer));
            parser.DebugFlag = DebugFlag;
            parser.Trace = false;
            parser.RemoveErrorListeners();
            parser.AddErrorListener(new MFSHErrorListenerParser(this.Mfsh,
                "MFsh Parser",
                sourceName,
                inputLines));
            //parser.ErrorHandler = new BailErrorStrategy();

            Parser.MFSHVisitor visitor = new Parser.MFSHVisitor(this, sourceName);
            visitor.DebugFlag = DebugFlag;
            visitor.Visit(parser.document());
            if (visitor.state.Count != 1)
            {
                String fullMsg = $"Error processing {sourceName}. Unterminated #{{Command}}";
                this.Mfsh.ConversionError("mfsh", "ProcessInclude", fullMsg);
            }

            ParseBlock block = visitor.Current;
            MIPreFsh retVal = new MIPreFsh(relativePath, 0)
            {
                Items = block.Items,
                RelativePath = relativePath
            };

            this.Fsh.Add(retVal);
            return retVal;
        }
    }
}
