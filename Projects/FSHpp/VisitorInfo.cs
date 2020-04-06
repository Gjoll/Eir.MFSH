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
        public class InputBlock
        {
            public String Text;
            public Int32 Index = 0;

            public InputBlock(String text,
                Int32 index)
            {
                this.Text = text;
                this.Index = index;
            }
        }

        public Stack<InputBlock> inputStack = new Stack<InputBlock>();
        public InputBlock Input => this.inputStack.Peek();

        public VisitorInfo(String input)
        {
            this.inputStack.Push(new InputBlock(input, 0));
        }

        /// <summary>
        /// Push the indicated substring of the current input as the
        /// new input block.
        /// </summary>
        public void PushSubString(Int32 index, Int32 length)
        {
            String subString = this.Input.Text.Substring(index, length);
            this.inputStack.Push(new InputBlock(subString, this.Input.Index));
        }

        public void PopSubString()
        {
            this.inputStack.Pop();
        }

        void CLog(String name)
        {
            Int32 CurrentLineNum()
            {
                Int32 retVal = 0;
                for (Int32 i = 0; i < this.Input.Index; i++)
                    if (this.Input.Text[i] == '\n')
                        retVal += 1;
                return retVal;
            }

            String StartOfLine()
            {
                Int32 i = this.Input.Index - 1;
                StringBuilder s = new StringBuilder();
                while ((i > 0) && (this.Input.Text[i] != '\n'))
                {
                    s.Insert(0, this.Input.Text[i]);
                    i -= 1;
                }
                return s.ToString();
            }

            String EndOfLine()
            {
                Int32 i = this.Input.Index + 1;
                StringBuilder s = new StringBuilder();
                while ((i < this.Input.Text.Length) && (this.Input.Text[i] != '\n'))
                {
                    s.Append(this.Input.Text[i]);
                    i += 1;
                }
                return s.ToString();
            }

            String start = StartOfLine();
            String end = EndOfLine();
            String current;
            if (this.Input.Index == this.Input.Text.Length)
            {
                current = "<eof>";
            }
            else
            {
                current = this.Input.Text[this.Input.Index].ToString();
                if (current == "\n")
                    current = "\\n";
            }

            Trace.WriteLine($"{name}: '{start}|{current}|{end}'");
            Trace.WriteLine($"        Line {CurrentLineNum()}, Index {this.Input.Index}");
        }

        public String CopyToEnd()
        {
            String retVal = this.Input.Text.Substring(this.Input.Index);
            this.Input.Index += retVal.Length;
            return retVal;
        }

        public void SkipBytes(Int32 newIndex)
        {
            this.Input.Index = newIndex;
        }

        public T GetCode<T>(String name,
            ParserRuleContext context)
            where T : NodeBase , new()
        {
            T retVal = new T();
            retVal.Comments = GetComments(context);

            retVal.Code = this.Input.Text.Substring(context.Start.StartIndex,
                context.Stop.StopIndex - context.Start.StartIndex + 1);
            Int32 stopNextIndex = context.Stop.StopIndex + 1;
            if (this.Input.Index < stopNextIndex)
                this.Input.Index = stopNextIndex;

            CLog(name);
            return retVal;
        }

        public String GetComments(ParserRuleContext context)
        {
            String comments = "";
            if (context.Start.StartIndex > this.Input.Index)
            {
                Int32 length = context.Start.StartIndex - this.Input.Index;
                comments = this.Input.Text.Substring(this.Input.Index, length);
                this.Input.Index = context.Start.StartIndex;
            }
            return comments;
        }

        public void End(String name,
            ParserRuleContext context)
        {
            this.Input.Index = context.Stop.StopIndex + 1;
            CLog(name);
        }

    }
}
