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
        public void PassThroughTest()
        {
            String input = File.ReadAllText("test1.fsh");
            input = input.Replace("\r", "");

            FSHpp pp = new FSHpp();
            NodeDocument d = pp.ProcessInput(input);
            String output = d.ToFSH();
            Debug.Assert(String.Compare(input, output) == 0);
        }
    }
}
