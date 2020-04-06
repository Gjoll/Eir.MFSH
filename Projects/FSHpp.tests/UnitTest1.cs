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


        void PassThrough(String path)
        {
            String input = File.ReadAllText(path);
            input = input.Replace("\r", "");

            FSHpp pp = new FSHpp();
            NodeDocument d = pp.ProcessInput(input);
            String output = d.ToFSH();

            Int32 i = 0;
            while (i < input.Length && i < output.Length)
            {
                if (input[i] != output[i])
                    break;
                i += 1;
            }


            if ((i < input.Length) || (i < output.Length))
            {
                Int32 start1 = i;
                while ((start1 > 0) && (input[start1] != '\n'))
                    start1 -= 1;

                Int32 len1 = input.IndexOf('\n', start1);
                if (len1 < 0)
                    len1 = input.Length;

                Int32 start2 = i;
                while ((start2 > 0) && (input[start2] != '\n'))
                    start2 -= 1;

                Int32 len2 = output.IndexOf('\n', start2);
                if (len2 < 0)
                    len2 = output.Length;

                Trace.WriteLine(input.Substring(start1, len1 - start1));
                Trace.WriteLine(output.Substring(start2, len2 - start2));
            }

            Debug.Assert(String.Compare(input, output) == 0);
        }

        [Fact]
        public void PassThroughTest()
        {
            PassThrough("test1.fsh");
        }
    }
}
