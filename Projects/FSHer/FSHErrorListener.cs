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

namespace Eir.FSHer
{
    class ErrorListener
    {
        String sourceName;
        private FSHer fsher;

        public ErrorListener(FSHer fsher,
            String sourceName)
        {
            this.sourceName = sourceName;
            this.fsher = fsher;
        }

        public void Error(IRecognizer recognizer,
            Int32 line,
            Int32 charPositionInLine,
            String msg,
            RecognitionException e)
        {
            line -= 1;
            String msgLine = null;
            if ((line >= 0) && (line < this.fsher.InputLines.Length))
            {
                String inputLine = this.fsher.InputLines[line];
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
                    sb.Append(inputLine.Substring(charPositionInLine+1));
                }
                msgLine = sb.ToString();
            }

            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"Error in parser at line {line}, column {charPositionInLine}");
                if (msgLine != null)
                    sb.AppendLine(msgLine);
                sb.AppendLine(msg);

                this.fsher.ConversionError("FSHer",
                    sourceName,
                    sb.ToString());
            }
        }
    }
    class FSHErrorListenerLexer : ErrorListener, IAntlrErrorListener<Int32>
    {
        public FSHErrorListenerLexer(FSHer fsher,
            String sourceName) : base(fsher, sourceName)
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
            Error(recognizer, line, charPositionInLine, msg, e);
        }
    }


    class FSHErrorListenerParser : ErrorListener, IAntlrErrorListener<IToken>
    {
        public FSHErrorListenerParser(FSHer fsher,
            String sourceName) : base(fsher, sourceName)
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
            Error(recognizer, line, charPositionInLine, msg, e);
        }
    }
}
