using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Dfa;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Sharpen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eir.MFSH.Parser
{
    class ErrorListener
    {
        String parserName;
        String sourceName;
        private MFsh mFsh;
        private String[] inputLines;

        public ErrorListener(MFsh mFsh,
            String parserName,
            String sourceName,
            String[] inputLines)
        {
            this.parserName = parserName;
            this.sourceName = sourceName;
            this.mFsh = mFsh;
            this.inputLines = inputLines;
        }

        public void Error(IRecognizer recognizer,
            Int32 line,
            Int32 charPositionInLine,
            String msg,
            RecognitionException e)
        {
            String msgLine = null;
            if ((line > 0) && (line <= this.inputLines.Length))
            {
                String inputLine = this.inputLines[line-1];
                if (charPositionInLine < 0)
                    charPositionInLine = 0;
                if (charPositionInLine > inputLine.Length)
                    charPositionInLine = inputLine.Length;
                StringBuilder sb = new StringBuilder();
                sb.Append(inputLine.Substring(0, charPositionInLine));
                sb.Append("-->");
                if (charPositionInLine < inputLine.Length)
                {
                    sb.Append(inputLine[charPositionInLine]);
                    sb.Append("<--");
                    sb.Append(inputLine.Substring(charPositionInLine + 1));
                }
                msgLine = sb.ToString();
            }

            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"Error: Error in {this.parserName} at line {line}, column {charPositionInLine}");
                if (msgLine != null)
                    sb.AppendLine(msgLine);
                sb.AppendLine(msg);

                this.mFsh.ConversionError("mFsh",
                    this.sourceName,
                    sb.ToString());
            }
        }
    }
    class MFSHErrorListenerLexer : ErrorListener, IAntlrErrorListener<Int32>
    {
        public MFSHErrorListenerLexer(MFsh mFsh,
            String parserName,
            String sourceName,
            String[] inputLines) : base(mFsh, parserName, sourceName, inputLines)
        {
        }

        public void SyntaxError(TextWriter output,
            IRecognizer recognizer,
            Int32 offendingSymbol,
            Int32 line,
            Int32 charPositionInLine,
            String msg,
            RecognitionException e)
        {
            this.Error(recognizer, line, charPositionInLine, msg, e);
        }
    }


    class MFSHErrorListenerParser : ErrorListener, IAntlrErrorListener<IToken>
    {
        public MFSHErrorListenerParser(MFsh mFsh,
            String parserName,
            String sourceName,
            String[] inputLines) : base(mFsh, parserName, sourceName, inputLines)
        {
        }

        public void SyntaxError(TextWriter output,
        IRecognizer recognizer,
        IToken offendingSymbol,
        Int32 line,
        Int32 charPositionInLine,
        String msg,
        RecognitionException e)
        {
            this.Error(recognizer, line, charPositionInLine, msg, e);
        }
    }
}
