using MFSH;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata;
using System.Text;
using Xunit;

namespace Eir.FSHer.tests
{
    public class UnitTest1
    {
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

        [Fact]
        public void Include1()
        {
            String input = GetCleanText("IncludeTest1.mfsh");
            MFsh pp = new MFsh();
            pp.TraceLogging(true, true, true);
            String results = pp.Parse(input, "test");
            Assert.True(pp.HasErrors == false);
            String shouldBe = File.ReadAllText("IncludeTest1.results");
            shouldBe = shouldBe.Trim().Replace("\r", "");
            results = results.Trim().Replace("\r", "");
            Assert.True(String.Compare(results, shouldBe) == 0);
        }


        [Fact]
        public void Define1()
        {
            String input = GetCleanText("DefineTest1.mfsh");
            MFsh pp = new MFsh();
            pp.TraceLogging(true, true, true);
            String results = pp.Parse(input, "test");
            Assert.True(pp.HasErrors == false);
            String shouldBe = File.ReadAllText("DefineTest1.results");
            shouldBe = shouldBe.Trim().Replace("\r", "");
            results = results.Trim().Replace("\r", "");
            Assert.True(String.Compare(results, shouldBe) == 0);
        }

        [Fact]
        public void Parse1()
        {
            String input = GetCleanText("Parse1.mfsh");
            MFsh pp = new MFsh();
            pp.TraceLogging(true, true, true);
            String results = pp.Parse(input, "test");
            Assert.True(pp.HasErrors == false);
        }

        [Fact]
        public void ParseMultiLine()
        {
            String input = GetCleanText("ParseMultiLine.mfsh");
            MFsh pp = new MFsh();
            pp.DebugFlag = true;
            pp.TraceLogging(true, true, true);
            String results = pp.Parse(input, "test");
            Assert.True(pp.HasErrors == false);
        }

    }
}
