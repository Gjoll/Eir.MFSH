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


        void ParseTest(String mfshFile, String resultsFile)
        {
            MFsh mfsh = CreateMfsh();
            mfsh.TraceLogging(true, true, true);
            mfsh.Load(TestFile(mfshFile));
            mfsh.Process();
            Assert.True(mfsh.HasErrors == false);
            Assert.True(mfsh.Mgr.Fsh.Count == 1);
            String shouldBe = this.GetCleanText(resultsFile);
            Assert.True(mfsh.TryGetText(mfshFile, out String fsh));
        }

        [Fact]
        public void MacroTest1()
        {
            {
                MFsh mfsh = CreateMfsh();
                mfsh.Load(TestFile("MFshMacroTest1A.mfsh"));
                mfsh.Load(TestFile("MFshMacroTest1B.mfsh"));
                mfsh.Process();

                StringBuilder sb = new StringBuilder();
                sb.Append("Line 1\n");
                sb.Append("Line 2\n");
                sb.Append("Line 3\n");
                Assert.True(mfsh.TryGetText("MFshMacroTest1B.mfsh", out String fsh));
                Assert.True(fsh == sb.ToString());
            }

            {
                MFsh mfsh = CreateMfsh();
                mfsh.Load(TestFile("MFshMacroTest1A.mfsh"));
                mfsh.Load(TestFile("MFshMacroTest1C.mfsh"));
                mfsh.Process();

                StringBuilder sb = new StringBuilder();
                sb.Append("Line 1\n");
                sb.Append("Line 2\n");
                sb.Append("Line 3\n");
                Assert.True(mfsh.TryGetText("MFshMacroTest1C.mfsh", out String fsh));
                Assert.True(fsh == sb.ToString());
            }
        }

        [Fact]
        public void MacroTest2()
        {
            MFsh mfsh = CreateMfsh();
            mfsh.Load(TestFile("MFshMacroTest2.mfsh"));
            mfsh.Process();

            StringBuilder sb = new StringBuilder();
            sb.Append("Line one\n");
            sb.Append("Line two\n");
            sb.Append("Line three\n");
            Assert.True(mfsh.TryGetText("MFshMacroTest2.mfsh", out String fsh));
            Assert.True(fsh == sb.ToString());
        }

        [Fact]
        public void MfshExpandVar1() => ParseTest("MfshExpandVar1.mfsh", "MfshExpandVar1.results");
    }
}
