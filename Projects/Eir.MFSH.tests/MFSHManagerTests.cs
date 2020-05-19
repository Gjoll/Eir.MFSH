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

        MIPreFsh ParseTest(String mfshFile, out MFshManager mgr)
        {
            String input = GetCleanText(mfshFile);
            MFsh mfsh = new MFsh();
            mfsh.TraceLogging(true, true, true);
            mgr = new MFshManager(mfsh);
            MIPreFsh b = mgr.ParseOne(input, "test", null);
            Assert.True(mfsh.HasErrors == false);
            return b;
        }

        [Fact]
        public void MgrMacroTest1()
        {
            MIPreFsh b = ParseTest("MgrMacroTest1.mfsh", out MFshManager mgr);
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
            MIPreFsh b = ParseTest("MgrMacroTest2.mfsh", out MFshManager mgr);
            Debug.Assert(((MIText)b.Items[0]).Line == "Line 1");
        }

        [Fact]
        public void MgrMacroRedirectTest()
        {
            MIPreFsh b = ParseTest("MgrMacroRedirectTest.mfsh", out MFshManager mgr);
            Debug.Assert(mgr.TryGetMacro("Macro1", out MIMacro macro1));
            Debug.Assert(macro1.Redirect == @"A\B.txt");
        }

        [Fact]
        public void MgrMacroParametersTest()
        {
            MIPreFsh b = ParseTest("MgrMacroParametersTest.mfsh", out MFshManager mgr);
            Debug.Assert(mgr.TryGetMacro("Macro1", out MIMacro macro1));
            Debug.Assert(macro1.Parameters.Count == 3);
            Debug.Assert(macro1.Parameters[0] == "Param1");
            Debug.Assert(macro1.Parameters[1] == "Param2");
            Debug.Assert(macro1.Parameters[2] == "Param3");
        }

        [Fact]
        public void MgrApplyTest()
        {
            MIPreFsh b = ParseTest("MgrApplyTest1.mfsh", out MFshManager mgr);
            Debug.Assert(b.Items.Count == 3);

            MIApply apply1 = (MIApply)b.Items[0];
            Debug.Assert(apply1.Name == "Macro1");
            Debug.Assert(apply1.Parameters.Count == 0);
            Debug.Assert(apply1.OnceFlag == false);

            MIApply apply2 = (MIApply)b.Items[1];
            Debug.Assert(apply2.Name == "Macro2");
            Debug.Assert(apply2.Parameters.Count == 0);
            Debug.Assert(apply2.OnceFlag == true);

            MIApply apply3 = (MIApply)b.Items[2];
            Debug.Assert(apply3.Name == "Macro3");
            Debug.Assert(apply3.Parameters.Count == 3);
            Debug.Assert(apply3.Parameters[0] == "a");
            Debug.Assert(apply3.Parameters[1] == "bb");
            Debug.Assert(apply3.Parameters[2] == "ccc");
            Debug.Assert(apply3.OnceFlag == false);
        }

        [Fact]
        public void MgrIncompatibleTest()
        {
            MIPreFsh b = ParseTest("MgrIncompatibleTest.mfsh", out MFshManager mgr);
            Debug.Assert(b.Items.Count == 1);

            MIIncompatible apply1 = (MIIncompatible)b.Items[0];
            Debug.Assert(apply1.Name == "Macro1");
        }
    }
}
