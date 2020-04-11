using System;
using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata;
using Xunit;

namespace FSHpp.tests
{
    public class UnitTest1
    {
        [Fact]
        public void PassThroughCovid19()
        {
            foreach (String fileName in Directory.GetFiles(@"C:\Development\covid-19\fsh\fsh-source"))
                PassThrough(fileName);
        }

        /// <summary>
        /// Getx text w/o carriage returns. Makes comparisons easier.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        String GetCleanText(String path)
        {
            String input = File.ReadAllText(path);
            input = input.Replace("\r", "");
            return input;
        }
        void ShowChunk(String text, Int32 index)
        {
            Int32 PrevIndex(Int32 count)
            {
                Int32 start = index;
                if (start >= text.Length)
                    start = text.Length - 1;

                while (count-- > 0)
                {
                    start = text.LastIndexOf('\n', start);
                    if (start < 0)
                        return 0;
                    start = start - 1;
                }

                return start + 1;
            }

            Int32 NextIndex(Int32 count)
            {
                Int32 next = index;
                if (next >= text.Length)
                    next = text.Length - 1;

                while (count-- > 0)
                {
                    next = text.IndexOf('\n', next);
                    if (next < 0)
                        return text.Length;
                    next = next + 1;
                }

                return next - 1;
            }

            Int32 startIndex = PrevIndex(3);
            Int32 nextIndex = NextIndex(3);
            Trace.WriteLine(text.Substring(startIndex, nextIndex - startIndex));
        }

        void PassThrough(String path)
        {
            String input = GetCleanText(path);

            FSHpp pp = new FSHpp();
            FSHpp.FSHFile f = pp.Parse(input);
            String output = f.Doc.ToFSH();

            Int32 i = 0;
            while (i < input.Length && i < output.Length)
            {
                if (input[i] != output[i])
                    break;
                i += 1;
            }

            Trace.WriteLine(f.Doc.Dump("*  "));

            if ((i < input.Length) || (i < output.Length))
            {
                Trace.WriteLine("---------------------------");
                ShowChunk(input, i);
                Trace.WriteLine("---------------------------");
                ShowChunk(output, i);
                Trace.WriteLine("---------------------------");
            }

            Debug.Assert(String.Compare(input, output) == 0);
        }

        [Fact]
        public void PassThroughTest()
        {
            PassThrough("test1.fsh");
        }
        [Fact]
        public void Macro1()
        {
            String input = GetCleanText("MacroTest1.fsh");
            FSHpp pp = new FSHpp();
            FSHpp.FSHFile f = pp.Parse(input);
            pp.Process();
        }

    }
}
