using MFSH;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace MFSH.Tests
{
    public class MFSHTests
    {
        /// <summary>
        /// Getx text w/o carriage returns. Makes comparisons easier.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        String GetCleanText(String path)
        {
            String input = File.ReadAllText(Path.Combine("TestFiles", path));
            input = input.Replace("\r", "");
            return input;
        }

        void PreParseTest(String mfshFile, String resultsFile)
        {
            String input = GetCleanText(mfshFile);
            MFsh pp = new MFsh();
            pp.TraceLogging(true, true, true);
            String results = pp.PreParseText(input, "test");
            Assert.True(pp.HasErrors == false);
            if (resultsFile == null)
                return;

            String shouldBe = File.ReadAllText(resultsFile);
            shouldBe = shouldBe.Trim().Replace("\r", "");
            results = results.Trim().Replace("\r", "");
            Assert.True(String.Compare(results, shouldBe) == 0);
        }

        void ParseTest(String mfshFile, String resultsFile)
        {
            String input = GetCleanText(mfshFile);
            MFsh pp = new MFsh();
            pp.TraceLogging(true, true, true);
            String results = pp.Parse(input, "test", null).Data.Text();
            Assert.True(pp.HasErrors == false);
            if (resultsFile == null)
                return;

            //$String shouldBe = File.ReadAllText(resultsFile);
            //$shouldBe = shouldBe.Trim().Replace("\r", "");
            //$results = results.Trim().Replace("\r", "");
            //$Assert.True(String.Compare(results, shouldBe) == 0);
        }

        //$[Fact]
        //public void Include1()
        //{
        //    String input = GetCleanText("IncludeTest1.mfsh");
        //    MFsh pp = new MFsh();
        //    pp.TraceLogging(true, true, true);
        //    String results = pp.Parse(input, "test", null).Data.Text();
        //    Assert.True(pp.HasErrors == false);
        //    String shouldBe = File.ReadAllText("IncludeTest1.results");
        //    shouldBe = shouldBe.Trim().Replace("\r", "");
        //    results = results.Trim().Replace("\r", "");
        //    Assert.True(String.Compare(results, shouldBe) == 0);
        //}

        [Fact]
        public void PreParse1() => PreParseTest("PreParse1.mfsh", "PreParse1.results");

        [Fact]
        public void Define1() => ParseTest("DefineTest1.mfsh", "DefineTest1.results");

        [Fact]
        public void Define2() => ParseTest("DefineTest2.mfsh", "DefineTest2.results");

        [Fact]
        public void Parse1() => ParseTest("Parse1.mfsh", null);

        [Fact]
        public void Parse2() => ParseTest("Parse2.mfsh", null);

        [Fact]
        public void Parse3() => ParseTest("Parse3.mfsh", null);
        [Fact]
        public void Parse4() => ParseTest("Parse4.mfsh", null);

        [Fact]
        public void Parse5() => ParseTest("Parse5.mfsh", null);
        [Fact]
        public void ExpandVar1() => ParseTest("ExpandVar1.mfsh", "ExpandVar1.results");

        [Fact]
        public void ExpandVar2() =>  ParseTest("ExpandVar2.mfsh", "ExpandVar2.results");

        [Fact]
        public void ParseRedirect() => ParseTest("ParseRedirect1.mfsh", "ParseRedirect1.results");

        [Fact]
        public void ParseMultiLine1() => ParseTest("ParseMultiLine1.mfsh", "ParseMultiLine1.results");

        [Fact]
        public void ParseMultiLine2() => ParseTest("ParseMultiLine2.mfsh", "ParseMultiLine2.results");

        [Fact]
        public void Profile1() => ParseTest("ParseProfile1.mfsh", null);

        [Fact]
        public void ApplyOnce1() => ParseTest("ApplyOnce1.mfsh", "ApplyOnce1.results");

        void ParseTestSingleError(String fileName, String errorex)
        {
            String input = GetCleanText(fileName);
            MFsh pp = new MFsh();
            pp.TraceLogging(true, true, true);
            String results = pp.Parse(input, "test", null).Data.Text();
            Assert.True(pp.HasErrors == true);
            String error = pp.Errors.First();
            Regex r = new Regex(errorex);
            Assert.True(r.IsMatch(error) == true);
        }

        [Fact]
        public void Incompatible1() => ParseTestSingleError("Incompatible1.mfsh", "Incompatible macro Alpha has already been applied");
        [Fact]
        public void Incompatible2() => ParseTestSingleError("Incompatible2.mfsh", "Incompatible macro Alpha has already been applied");

        [Fact]
        public void ApplyOnce2() => ParseTestSingleError("ApplyOnce2.mfsh",
            "Attempt to apply macro Alpha with once and variables");

        [Fact]
        public void ApplyOnce3() => ParseTestSingleError("ApplyOnce3.mfsh",
                                                    "Attempt to call macro Alpha with once = true, and previous call with once = false");

        [Fact]
        public void ApplyOnce4() => ParseTest("ApplyOnce4.mfsh", "ApplyOnce4.results");
    }
}
