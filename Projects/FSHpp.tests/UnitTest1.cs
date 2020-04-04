using System;
using System.Diagnostics;
using System.IO;
using Xunit;

namespace FSHpp.tests
{
    public class UnitTest1
    {
        [Fact]
        public void PassThroughTest()
        {
            String input = File.ReadAllText("test1.fsh");
            FSHpp pp = new FSHpp();
            String output = pp.ProcessInput(input);
            Debug.Assert(String.Compare(input, output) == 0);
        }
    }
}
