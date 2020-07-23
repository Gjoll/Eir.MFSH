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
            Assert.True(b.Items.Count == 3);
            Assert.True(((MIText)b.Items[0]).Line == "Line 1\n");
            Assert.True(((MIText)b.Items[1]).Line == "Line 2\n");
            Assert.True(((MIText)b.Items[2]).Line == "Line 5\n");

            Assert.True(mfsh.MacroMgr.TryGetItem(null, "Macro1", out MIApplicable macro1));
            Assert.True(macro1.Items.Count == 2);
            Assert.True(((MIText)macro1.Items[0]).Line == "    Line 3\n");
            Assert.True(((MIText)macro1.Items[1]).Line == "    Line 4\n");

            Assert.True(mfsh.MacroMgr.TryGetItem(null, "Macro2", out MIApplicable macro2));
            Assert.True(mfsh.MacroMgr.TryGetItem(null, "Macro3", out MIApplicable macro3));
        }

        [Fact]
        public void MgrMacroTest2()
        {
            MIPreFsh b = ParseTest("MgrMacroTest2.mfsh", out MFsh mfsh);
            Assert.True(((MIText)b.Items[0]).Line == "Line 1");
        }

        [Fact]
        public void MgrMacroRedirectTest()
        {
            MIPreFsh b = ParseTest("MgrMacroRedirectTest.mfsh", out MFsh mfsh);
            if (mfsh.MacroMgr.TryGetItem(null, "Macro1", out MIApplicable item1) == false)
            {
                Assert.True(false);
                throw new NotImplementedException();
            }
            MIMacro macro1 = (MIMacro)item1;
            Assert.True(macro1.Redirect == @"A\B.txt");
            Assert.True(macro1.UniqueFlag == MIApplicable.UniqueFlags.Profile);
        }

        [Fact]
        public void MgrMacroParametersTest()
        {
            MIPreFsh b = ParseTest("MgrMacroParametersTest.mfsh", out MFsh mfsh);
            Assert.True(mfsh.MacroMgr.TryGetItem(null, "Macro1", out MIApplicable item1));
            MIMacro macro1 = (MIMacro)item1;
            Assert.True(macro1.UniqueFlag == MIApplicable.UniqueFlags.Always);
            Assert.True(macro1.Parameters.Count == 3);
            Assert.True(macro1.Parameters[0] == "Param1");
            Assert.True(macro1.Parameters[1] == "Param2");
            Assert.True(macro1.Parameters[2] == "Param3");
        }

        [Fact]
        public void MgrApplyTest()
        {
            MIPreFsh b = ParseTest("MgrApplyTest1.mfsh", out MFsh mfsh);
            Assert.True(b.Items.Count == 3);

            MIApply apply1 = (MIApply)b.Items[0];
            Assert.True(apply1.Name == "Macro1");
            Assert.True(apply1.Parameters.Count == 0);

            MIApply apply2 = (MIApply)b.Items[1];
            Assert.True(apply2.Name == "Macro2");
            Assert.True(apply2.Parameters.Count == 0);

            MIApply apply3 = (MIApply)b.Items[2];
            Assert.True(apply3.Name == "Macro3");
            Assert.True(apply3.Parameters.Count == 3);
            Assert.True(apply3.Parameters[0] == "a");
            Assert.True(apply3.Parameters[1] == "bb");
            Assert.True(apply3.Parameters[2] == "ccc");
        }

        [Fact]
        public void MgrIncompatibleTest()
        {
            MIPreFsh b = ParseTest("MgrIncompatibleTest.mfsh", out MFsh mfsh);
            Assert.True(b.Items.Count == 1);

            MIIncompatible apply1 = (MIIncompatible)b.Items[0];
            Assert.True(apply1.Name == "Macro1");
        }
    }
}
