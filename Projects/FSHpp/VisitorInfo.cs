using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp
{
    public class VisitorInfo
    {
        public String Input;
        public Int32 InputIndex = 0;

        void CLog(String name)
        {
            Int32 CurrentLineNum()
            {
                Int32 retVal = 0;
                for (Int32 i = 0; i < this.InputIndex; i++)
                    if (this.Input[i] == '\n')
                        retVal += 1;
                return retVal;
            }

            String StartOfLine()
            {
                Int32 i = this.InputIndex - 1;
                StringBuilder s = new StringBuilder();
                while ((i > 0) && (this.Input[i] != '\n'))
                {
                    s.Insert(0, this.Input[i]);
                    i -= 1;
                }
                return s.ToString();
            }

            String EndOfLine()
            {
                Int32 i = this.InputIndex + 1;
                StringBuilder s = new StringBuilder();
                while ((i < this.Input.Length) && (this.Input[i] != '\n'))
                {
                    s.Append(this.Input[i]);
                    i += 1;
                }
                return s.ToString();
            }

            String start = StartOfLine();
            String end = EndOfLine();
            String current;
            if (this.InputIndex == this.Input.Length)
            {
                current = "<eof>";
            }
            else
            {
                current = this.Input[this.InputIndex].ToString();
                if (current == "\n")
                    current = "\\n";
            }

            Trace.WriteLine($"{name}: '{start}|{current}|{end}'");
            Trace.WriteLine($"        Line {CurrentLineNum()}, Index {this.InputIndex}");
        }

        public String CopyToEnd()
        {
            String retVal = this.Input.Substring(this.InputIndex);
            this.InputIndex += retVal.Length;
            return retVal;
        }

        public void SkipBytes(Int32 newIndex)
        {
            this.InputIndex = newIndex;
        }

        public T GetCode<T>(String name,
            ParserRuleContext context)
            where T : NodeBase , new()
        {
            T retVal = new T();
            retVal.Comments = GetComments(context);

            retVal.Code = this.Input.Substring(context.Start.StartIndex,
                context.Stop.StopIndex - context.Start.StartIndex + 1);
            this.InputIndex = context.Stop.StopIndex + 1;
            CLog(name);
            return retVal;
        }

        public String GetComments(ParserRuleContext context)
        {
            String comments = "";
            if (context.Start.StartIndex > this.InputIndex)
            {
                Int32 length = context.Start.StartIndex - this.InputIndex;
                comments = this.Input.Substring(this.InputIndex, length);
                this.InputIndex = context.Start.StartIndex;
            }
            return comments;
        }

        public void End(String name,
            ParserRuleContext context)
        {
            this.InputIndex = context.Stop.StopIndex + 1;
            CLog(name);
        }

    }
}
