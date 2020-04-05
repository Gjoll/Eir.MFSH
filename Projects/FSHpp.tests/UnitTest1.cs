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
            Debug.Assert(String.Compare(input, output) == 0);
        }

        [Fact]
        public void PassThroughTest()
        {
            PassThrough("test1.fsh");
        }
    }
}
