using MFSH;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using MFSH.Parser2;
using Xunit;

namespace MFSH.Tests
{
    public class MFSHManagerTests
    {
        /// <summary>
        /// Get text w/o carriage returns. Makes comparisons easier.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        String GetCleanText(String path)
        {
            String input = File.ReadAllText(Path.Combine("TestFiles", path));
            input = input.Replace("\r", "");
            return input;
        }

        ParseBlock ParseTest(String mfshFile)
        {
            String input = GetCleanText(mfshFile);
            MFsh mfsh = new MFsh();
            mfsh.TraceLogging(true, true, true);
            MFshManager mgr = new MFshManager(mfsh);
            ParseBlock b = mgr.ParseOne(input, "test", null);
            Assert.True(mfsh.HasErrors == false);
            return b;
        }

        [Fact]
        public void MFSHManagerTest1()
        {
            ParseBlock b = ParseTest("MFSHManagerTest1.mfsh");
        }
    }
}
