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
            Assert.True(mfsh.TryGetText($"{testFile}.fsh", out String actualResults));
            Assert.True(String.Compare(actualResults, shouldBe) == 0);
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
                Assert.True(mfsh.TryGetText("MFshConditional1.fsh", out String fsh));
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
                Assert.True(mfsh.TryGetText("MFshConditional1.fsh", out String fsh));
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
                Assert.True(mfsh.TryGetText("MFshConditional2.fsh", out String fsh));
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
                Assert.True(mfsh.TryGetText("MFshConditional2.fsh", out String fsh));
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
                Assert.True(mfsh.TryGetText("MFshConditional2.fsh", out String fsh));
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
                Assert.True(mfsh.TryGetText("MFshConditional3.fsh", out String fsh));
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
                Assert.True(mfsh.TryGetText("MFshMacroTest1B.fsh", out String fsh));
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
                Assert.True(mfsh.TryGetText("MFshMacroTest1C.fsh", out String fsh));
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
            Assert.True(mfsh.TryGetText("MFshMacroTest2.fsh", out String fsh));
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
            sb.Append("\n");
            sb.Append("\n");
            sb.Append("\n");
            sb.Append("  Line one\n");
            sb.Append("  Line one\n");
            sb.Append("  Line two\n");
            sb.Append("  Line two\n");
            sb.Append("  Line three\n");
            sb.Append("  Line three\n");
            Assert.True(mfsh.TryGetText("MFshUseTest1.fsh", out String fsh));
            Assert.True(fsh == sb.ToString());
        }

        [Fact]
        public void MfshExpandVar1() => ParseTest("MfshExpandVar1", "MfshExpandVar1.results");

        [Fact]
        public void MfshTickTest() => ParseTest("MfshTickText", "MfshTickText.results");
    }
}
