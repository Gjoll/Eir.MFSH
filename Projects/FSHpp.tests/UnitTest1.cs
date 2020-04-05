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
            FSHpp pp = new FSHpp();
            NodeDocument d = new NodeDocument();
            pp.ProcessInput(d, input);
            //Debug.Assert(String.Compare(input, output) == 0);
        }
    }
}
