using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;
using Eir.MFSH;

namespace Eir.MFSH.Tests
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

        ParseBlock ParseTest(String mfshFile, out MFshManager mgr)
        {
            String input = GetCleanText(mfshFile);
            MFsh mfsh = new MFsh();
            mfsh.TraceLogging(true, true, true);
            mgr = new MFshManager(mfsh);
            ParseBlock b = mgr.ParseOne(input, "test", null);
            Assert.True(mfsh.HasErrors == false);
            return b;
        }

        [Fact]
        public void MgrMacroTest1()
        {
            ParseBlock b = ParseTest("MgrMacroTest1.mfsh", out MFshManager mgr);
            Debug.Assert(b.Items.Count == 3);
            Debug.Assert(((MIText)b.Items[0]).Line == "Line 1\n");
            Debug.Assert(((MIText)b.Items[1]).Line == "Line 2\n");
            Debug.Assert(((MIText)b.Items[2]).Line == "Line 5\n");

            Debug.Assert(mgr.TryGetMacro("Macro1", out MIMacro macro1));
            Debug.Assert(macro1.Items.Count == 2);
            Debug.Assert(((MIText)macro1.Items[0]).Line == "    Line 3\n");
            Debug.Assert(((MIText)macro1.Items[1]).Line == "    Line 4\n");

            Debug.Assert(mgr.TryGetMacro("Macro2", out MIMacro macro2));
            Debug.Assert(mgr.TryGetMacro("Macro3", out MIMacro macro3));
        }

        [Fact]
        public void MgrMacroTest2()
        {
            ParseBlock b = ParseTest("MgrMacroTest2.mfsh", out MFshManager mgr);
            Debug.Assert(((MIText)b.Items[0]).Line == "Line 1");
        }

        [Fact]
        public void MgrMacroParametersTest()
        {
            ParseBlock b = ParseTest("MgrMacroParametersTest.mfsh", out MFshManager mgr);
            Debug.Assert(mgr.TryGetMacro("Macro1", out MIMacro macro1));
            Debug.Assert(macro1.Parameters.Count == 3);
            Debug.Assert(macro1.Parameters[0] == "Param1");
            Debug.Assert(macro1.Parameters[1] == "Param2");
            Debug.Assert(macro1.Parameters[2] == "Param3");
        }

        [Fact]
        public void MgrApplyTest1()
        {
            ParseBlock b = ParseTest("MgrApplyTest1.mfsh", out MFshManager mgr);
            Debug.Assert(b.Items.Count == 3);
            Debug.Assert(((MIText)b.Items[0]).Line == "Line 1\n");
            Debug.Assert(((MIText)b.Items[1]).Line == "Line 2\n");
            Debug.Assert(((MIText)b.Items[2]).Line == "Line 5\n");

            Debug.Assert(mgr.TryGetMacro("Macro1", out MIMacro macro1));
            Debug.Assert(macro1.Items.Count == 2);
            Debug.Assert(((MIText)macro1.Items[0]).Line == "    Line 3\n");
            Debug.Assert(((MIText)macro1.Items[1]).Line == "    Line 4\n");

            Debug.Assert(mgr.TryGetMacro("Macro2", out MIMacro macro2));
            Debug.Assert(mgr.TryGetMacro("Macro3", out MIMacro macro3));
        }
    }
}
