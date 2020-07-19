using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;
using Eir.MFSH;
using Eir.MFSH.Parser;

namespace Eir.MFSH.Tests
{
    public class ParseManagerTests
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

        MIPreFsh ParseTest(String mfshFile, out MFsh mfsh)
        {
            String input = GetCleanText(mfshFile);
            mfsh = new MFsh();
            mfsh.TraceLogging(true, true, true);
            MIPreFsh b = mfsh.Parser.ParseOne(input, "test");
            Assert.True(mfsh.HasErrors == false);
            return b;
        }

        [Fact]
        public void MgrMacroTest1()
        {
            MIPreFsh b = ParseTest("MgrMacroTest1.mfsh", out MFsh mfsh);
            Debug.Assert(b.Items.Count == 3);
            Debug.Assert(((MIText)b.Items[0]).Line == "Line 1\n");
            Debug.Assert(((MIText)b.Items[1]).Line == "Line 2\n");
            Debug.Assert(((MIText)b.Items[2]).Line == "Line 5\n");

            Debug.Assert(mfsh.MacroMgr.TryGetItem(null, "Macro1", out MIApplicable macro1));
            Debug.Assert(macro1.Items.Count == 2);
            Debug.Assert(((MIText)macro1.Items[0]).Line == "    Line 3\n");
            Debug.Assert(((MIText)macro1.Items[1]).Line == "    Line 4\n");

            Debug.Assert(mfsh.MacroMgr.TryGetItem(null, "Macro2", out MIApplicable macro2));
            Debug.Assert(mfsh.MacroMgr.TryGetItem(null, "Macro3", out MIApplicable macro3));
        }

        [Fact]
        public void MgrMacroTest2()
        {
            MIPreFsh b = ParseTest("MgrMacroTest2.mfsh", out MFsh mfsh);
            Debug.Assert(((MIText)b.Items[0]).Line == "Line 1");
        }

        [Fact]
        public void MgrMacroRedirectTest()
        {
            MIPreFsh b = ParseTest("MgrMacroRedirectTest.mfsh", out MFsh mfsh);
            Debug.Assert(mfsh.MacroMgr.TryGetItem(null, "Macro1", out MIApplicable item1));
            MIMacro macro1 = (MIMacro)item1;
            Debug.Assert(macro1.Redirect == @"A\B.txt");
            Debug.Assert(macro1.OnceFlag == true);
        }

        [Fact]
        public void MgrMacroParametersTest()
        {
            MIPreFsh b = ParseTest("MgrMacroParametersTest.mfsh", out MFsh mfsh);
            Debug.Assert(mfsh.MacroMgr.TryGetItem(null, "Macro1", out MIApplicable item1));
            MIMacro macro1 = (MIMacro)item1;
            Debug.Assert(macro1.OnceFlag == false);
            Debug.Assert(macro1.Parameters.Count == 3);
            Debug.Assert(macro1.Parameters[0] == "Param1");
            Debug.Assert(macro1.Parameters[1] == "Param2");
            Debug.Assert(macro1.Parameters[2] == "Param3");
        }

        [Fact]
        public void MgrApplyTest()
        {
            MIPreFsh b = ParseTest("MgrApplyTest1.mfsh", out MFsh mfsh);
            Debug.Assert(b.Items.Count == 3);

            MIApply apply1 = (MIApply)b.Items[0];
            Debug.Assert(apply1.Name == "Macro1");
            Debug.Assert(apply1.Parameters.Count == 0);

            MIApply apply2 = (MIApply)b.Items[1];
            Debug.Assert(apply2.Name == "Macro2");
            Debug.Assert(apply2.Parameters.Count == 0);

            MIApply apply3 = (MIApply)b.Items[2];
            Debug.Assert(apply3.Name == "Macro3");
            Debug.Assert(apply3.Parameters.Count == 3);
            Debug.Assert(apply3.Parameters[0] == "a");
            Debug.Assert(apply3.Parameters[1] == "bb");
            Debug.Assert(apply3.Parameters[2] == "ccc");
        }

        [Fact]
        public void MgrIncompatibleTest()
        {
            MIPreFsh b = ParseTest("MgrIncompatibleTest.mfsh", out MFsh mfsh);
            Debug.Assert(b.Items.Count == 1);

            MIIncompatible apply1 = (MIIncompatible)b.Items[0];
            Debug.Assert(apply1.Name == "Macro1");
        }
    }
}
