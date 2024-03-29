using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace Eir.MFSH.Tests
{
    public class MFSHTests
    {
        String TestFile(String name) => Path.Combine("TestFiles", name);

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

        bool Compare(String actualResults, String shouldBe)
        {
            String[] actualArr = actualResults.Split('\n');
            String[] shouldBeArr = shouldBe.Split('\n');

            if (actualArr.Length != shouldBeArr.Length)
            {
                Trace.WriteLine($"Lengths differ: {actualArr.Length} {shouldBeArr}");
                return false;
            }

            for (Int32 i = 0; i < actualArr.Length; i++)
            {
                String actualLine = actualArr[i].Replace("\r", "").Trim();
                String shouldBeLine = actualArr[i].Replace("\r", "").Trim();
                if (String.Compare(actualLine, shouldBeLine) != 0)
                {
                    Trace.WriteLine($"Expected '{shouldBeLine}'");
                    Trace.WriteLine($"Actual   '{actualLine}'");
                    return false;
                }
            }
            return true;
        }

        MFsh CreateMfsh()
        {
            MFsh mfsh = new MFsh();
            mfsh.DebugFlag = true;
            mfsh.BaseInputDir = Path.GetFullPath("TestFiles");
            mfsh.BaseUrl = "http://www.test.com";
            mfsh.BaseOutputDir = @"c:\Temp\MFSHTests";
            return mfsh;
        }


        void ParseTest(String testFile, String resultsFile)
        {
            MFsh mfsh = CreateMfsh();
            mfsh.TraceLogging(true, true, true);
            mfsh.Load(TestFile($"{testFile}.mfsh"));
            CheckErrors(mfsh);
            mfsh.Process();
            CheckErrors(mfsh);
            Assert.True(mfsh.Parser.Fsh.Count == 1);
            String shouldBe = this.GetCleanText(resultsFile);
            Assert.True(mfsh.TryGetTextByRelativePath($"{testFile}.fsh", out String actualResults));
            Assert.True(Compare(actualResults, shouldBe));
        }

        void CheckErrors(MFsh mfsh)
        {
            if (mfsh.HasErrors == false)
                return;

            StringBuilder sb = new StringBuilder();
            mfsh.FormatErrorMessages(sb);
            Trace.WriteLine(sb.ToString());
            Assert.True(false);
        }

        [Fact]
        public void ConditionalTest1()
        {
            {
                MFsh mfsh = CreateMfsh();
                mfsh.GlobalVars.Add("CVar", "abc");
                mfsh.Load(TestFile("MFshConditional1.mfsh"));
                CheckErrors(mfsh);
                mfsh.Process();
                CheckErrors(mfsh);

                StringBuilder sb = new StringBuilder();
                sb.Append("Yes\n");
                Assert.True(mfsh.TryGetTextByRelativePath("MFshConditional1.fsh", out String fsh));
                Assert.True(fsh == sb.ToString());
            }

            {
                MFsh mfsh = CreateMfsh();
                mfsh.GlobalVars.Add("CVar", "def");
                mfsh.Load(TestFile("MFshConditional1.mfsh"));
                CheckErrors(mfsh);
                mfsh.Process();
                CheckErrors(mfsh);

                StringBuilder sb = new StringBuilder();
                sb.Append("No\n");
                Assert.True(mfsh.TryGetTextByRelativePath("MFshConditional1.fsh", out String fsh));
                Assert.True(fsh == sb.ToString());
            }
        }

        [Fact]
        public void ConditionalTest2()
        {
            {
                MFsh mfsh = CreateMfsh();
                mfsh.GlobalVars.Add("CVar", "abc");
                mfsh.Load(TestFile("MFshConditional2.mfsh"));
                CheckErrors(mfsh);
                mfsh.Process();
                CheckErrors(mfsh);

                StringBuilder sb = new StringBuilder();
                sb.Append("One\n");
                Assert.True(mfsh.TryGetTextByRelativePath("MFshConditional2.fsh", out String fsh));
                Assert.True(fsh == sb.ToString());
            }

            {
                MFsh mfsh = CreateMfsh();
                mfsh.GlobalVars.Add("CVar", "def");
                mfsh.Load(TestFile("MFshConditional2.mfsh"));
                CheckErrors(mfsh);
                mfsh.Process();
                CheckErrors(mfsh);

                StringBuilder sb = new StringBuilder();
                sb.Append("Two\n");
                Assert.True(mfsh.TryGetTextByRelativePath("MFshConditional2.fsh", out String fsh));
                Assert.True(fsh == sb.ToString());
            }


            {
                MFsh mfsh = CreateMfsh();
                mfsh.GlobalVars.Add("CVar", "ghi");
                mfsh.Load(TestFile("MFshConditional2.mfsh"));
                CheckErrors(mfsh);
                mfsh.Process();
                CheckErrors(mfsh);

                StringBuilder sb = new StringBuilder();
                sb.Append("Three\n");
                Assert.True(mfsh.TryGetTextByRelativePath("MFshConditional2.fsh", out String fsh));
                Assert.True(fsh == sb.ToString());
            }
        }

        [Fact]
        public void ConditionalTest3()
        {
            void Test(String val, String result)
            {
                MFsh mfsh = CreateMfsh();
                mfsh.GlobalVars.Add("CVar", val);
                mfsh.Load(TestFile("MFshConditional3.mfsh"));
                CheckErrors(mfsh);
                mfsh.Process();
                CheckErrors(mfsh);

                StringBuilder sb = new StringBuilder();
                sb.Append($"{result}\n");
                Assert.True(mfsh.TryGetTextByRelativePath("MFshConditional3.fsh", out String fsh));
                Assert.True(fsh == sb.ToString());
            }
            Test("4", "LT");
            Test("9", "LE");
            Test("8", "LE");

            Test("16", "GT");
            Test("12", "GE");
            Test("14", "GE");
        }


        [Fact]
        public void MacroTest1()
        {
            {
                MFsh mfsh = CreateMfsh();
                mfsh.Load(TestFile("MFshMacroTest1A.mfsh"));
                CheckErrors(mfsh);
                mfsh.Load(TestFile("MFshMacroTest1B.mfsh"));
                CheckErrors(mfsh);
                mfsh.Process();
                CheckErrors(mfsh);

                StringBuilder sb = new StringBuilder();
                sb.Append("Line 1\n");
                sb.Append("Line 2\n");
                sb.Append("Line 3\n");
                Assert.True(mfsh.TryGetTextByRelativePath("MFshMacroTest1B.fsh", out String fsh));
                Assert.True(fsh == sb.ToString());
            }

            {
                MFsh mfsh = CreateMfsh();
                mfsh.Load(TestFile("MFshMacroTest1A.mfsh"));
                CheckErrors(mfsh);
                mfsh.Load(TestFile("MFshMacroTest1C.mfsh"));
                CheckErrors(mfsh);
                mfsh.Process();
                CheckErrors(mfsh);

                StringBuilder sb = new StringBuilder();
                sb.Append("Line 1\n");
                sb.Append("Line 2\n");
                sb.Append("Line 3\n");
                Assert.True(mfsh.TryGetTextByRelativePath("MFshMacroTest1C.fsh", out String fsh));
                Assert.True(fsh == sb.ToString());
            }
        }

        [Fact]
        public void MFshMultiLineQuoteLeftAdjust()
        {
            //ParseTest("MFshMultiLineQuoteLeftAdjust", "MFshMultiLineQuoteLeftAdjust.results");

            MFsh mfsh = CreateMfsh();
            mfsh.FragDir = @"c:\Temp\FragDir";
            mfsh.FragTemplatePath = TestFile("FragmentTemplate.txt");
            mfsh.Load(TestFile("MFshMultiLineQuoteLeftAdjust.mfsh"));
            CheckErrors(mfsh);
            mfsh.Process();
            CheckErrors(mfsh);

            String shouldBe = this.GetCleanText("MFshMultiLineQuoteLeftAdjust.results");

            Assert.True(mfsh.TryGetFragTextByRelativePath($"MFshMultiLineQuoteLeftAdjust.fsh", out String actualResults));
            Assert.True(Compare(actualResults, shouldBe));
        }

        [Fact]
        public void FragTest1()
        {
            {
                MFsh mfsh = CreateMfsh();
                mfsh.Load(TestFile("MFshFragTest1A.mfsh"));
                CheckErrors(mfsh);
                mfsh.Load(TestFile("MFshFragTest1B.mfsh"));
                CheckErrors(mfsh);
                mfsh.Process();
                CheckErrors(mfsh);

                Assert.True(mfsh.MacroMgr.TryGetItem("Frag1", out MIApplicable item));
                MIFragment frag = (MIFragment)item;
                Assert.True(frag.Name == "Frag1");
                Assert.True(frag.Parent == "Observation");
                Assert.True(frag.Title == "Frag1 Title");
                Assert.True(frag.Description == "Frag1 Description");

                StringBuilder sb = new StringBuilder();
                sb.Append("Line 1\n");
                sb.Append("Line 2\n");
                sb.Append("Line 3\n");
                Assert.True(mfsh.TryGetTextByRelativePath("MFshFragTest1B.fsh", out String fsh));
                Assert.True(fsh == sb.ToString());
            }

            {
                MFsh mfsh = CreateMfsh();
                mfsh.Load(TestFile("MFshFragTest1A.mfsh"));
                CheckErrors(mfsh);
                mfsh.Load(TestFile("MFshFragTest1C.mfsh"));
                CheckErrors(mfsh);
                mfsh.Process();
                CheckErrors(mfsh);

                StringBuilder sb = new StringBuilder();
                sb.Append("Line 1\n");
                sb.Append("Line 2\n");
                sb.Append("Line 2\n");
                sb.Append("Line 2\n");
                sb.Append("Line 2\n");
                sb.Append("Line 3\n");
                Assert.True(mfsh.TryGetTextByRelativePath("MFshFragTest1C.fsh", out String fsh));
                Assert.True(fsh == sb.ToString());
            }
        }

        [Fact]
        public void MacroTest2()
        {
            MFsh mfsh = CreateMfsh();
            mfsh.Load(TestFile("MFshMacroTest2.mfsh"));
            CheckErrors(mfsh);
            mfsh.Process();
            CheckErrors(mfsh);

            StringBuilder sb = new StringBuilder();
            sb.Append("Line one\n");
            sb.Append("Line two\n");
            sb.Append("Line three\n");
            Assert.True(mfsh.TryGetTextByRelativePath("MFshMacroTest2.fsh", out String fsh));
            Assert.True(fsh == sb.ToString());
        }


        [Fact]
        public void UseTest2()
        {
            MFsh mfsh = CreateMfsh();
            mfsh.Load(TestFile("MFshUseTest1.mfsh"));
            CheckErrors(mfsh);
            mfsh.Process();
            CheckErrors(mfsh);

            StringBuilder sb = new StringBuilder();
            sb.Append("  Line one\n");
            sb.Append("  Line one\n");
            sb.Append("  Line two\n");
            sb.Append("  Line two\n");
            sb.Append("  Line three\n");
            sb.Append("  Line three\n");
            Assert.True(mfsh.TryGetTextByRelativePath("MFshUseTest1.fsh", out String fsh));
            Assert.True(fsh == sb.ToString());
        }

        [Fact]
        public void MfshExpandVar1() => ParseTest("MfshExpandVar1", "MfshExpandVar1.results");

        [Fact]
        public void MfshTickTest() => ParseTest("MfshTickText", "MfshTickText.results");
    }
}
