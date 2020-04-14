using System;
using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata;
using System.Text;
using Xunit;

namespace FSHer.tests
{
    public class UnitTest1
    {
        [Fact]
        public void PassThroughMCode()
        {
            foreach (String fileName in Directory.GetFiles(@"C:\Development\MITRE\fhir-mCODE-ig\fsh", "*.fsh"))
                PassThrough(fileName);
        }

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
                    if (start <= 0)
                        return 0;
                    start = start - 1;
                }

                return start + 1;
            }

            Int32 NextIndex(Int32 count)
            {
                Int32 next = index;
                if (next < 0)
                    return text.Length;

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

        void Compare(String results,
            FSHFile f)
        {
            results = results.Replace("\r", "");

            String output = f.Doc.ToFSH();

            Int32 i = 0;
            while (i < results.Length && i < output.Length)
            {
                if (results[i] != output[i])
                    break;
                i += 1;
            }

            //Trace.WriteLine(f.Doc.Dump("*  "));

            if ((i < results.Length) || (i < output.Length))
            {
                Trace.WriteLine("---------------------------");
                ShowChunk(results, i);
                Trace.WriteLine("---------------------------");
                ShowChunk(output, i);
                Trace.WriteLine("---------------------------");
            }
            Debug.Assert(String.Compare(results, output) == 0);
        }

        void PassThrough(String path)
        {
            String input = GetCleanText(path);

            FSHer pp = new FSHer();
            FSHFile f = pp.Parse(input, Path.GetFileName(path));
            if (pp.HasErrors == true)
            {
                StringBuilder sb = new StringBuilder();
                pp.FormatErrorMessages(sb);
                Trace.WriteLine(sb.ToString());
                Assert.True(false);
            }

            Compare(input, f);
        }

        [Fact]
        public void PassThroughTest()
        {
            PassThrough("test1.fsh");
        }

        [Fact]
        public void NodeCloneTest()
        {
            String input = GetCleanText("MacroTest1.fsh");
            FSHer pp = new FSHer();
            FSHFile f = pp.Parse(input, "test");
            NodeBase b = f.Doc.Clone();
            String output = b.ToFSH();
            Compare(output, f);
        }

        public void MacroTest(String inputFile, String resultsFile)
        {
            String input = GetCleanText(inputFile);
            FSHer pp = new FSHer();
            FSHFile f = pp.Parse(input, "test");
            //Trace.WriteLine(f.Doc.Dump("* "));
            if (pp.Process() == false)
            {
                StringBuilder sb = new StringBuilder();
                pp.FormatErrorMessages(sb);
                Trace.WriteLine(sb.ToString());
                Assert.True(false);
            }
            //Trace.WriteLine(f.Doc.Dump("* "));
            String expanded = f.Doc.ToFSH();
            String results = File.ReadAllText(resultsFile);
            Compare(results, f);
            //File.WriteAllText(@"c:\Temp\scr.txt", expanded);
        }


        [Fact]
        public void Macro1()
        {
            MacroTest("MacroTest1.fsh", "MacroTest1.results.txt");
        }

        [Fact]
        public void Macro2()
        {
            MacroTest("MacroTest2.fsh", "MacroTest2.results.txt");
        }

        // Macro expansion should fail because parent mismatch.
        [Fact]
        public void Macro3()
        {
            String input = GetCleanText("MacroTest3.fsh");
            FSHer pp = new FSHer();
            FSHFile f = pp.Parse(input, "test");
            Assert.True(pp.HasErrors == false);

            // shoudl return false cause or parent profile mismatch.
            Assert.True(pp.Process() == false);
        }
    }
}
