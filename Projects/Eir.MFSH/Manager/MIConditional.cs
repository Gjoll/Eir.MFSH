using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata;
using System.Text;
using Eir.MFSH;

namespace Eir.MFSH
{
    /// <summary>
    /// Macro definition
    /// </summary>
    [DebuggerDisplay("Conditional")]
    public class MIConditional : MIBase
    {
        public abstract class CState
        {
            public String Lhs;
            public abstract bool IsTrue(List<VariablesBlock> variableBlocks);
        }

        public class CStateTrue : CState
        {
            public override bool IsTrue(List<VariablesBlock> variableBlocks) => true;
        }

        public class CStateIsString : CState
        {
            public String Rhs;
            public override bool IsTrue(List<VariablesBlock> variableBlocks)
            {
                String rhs = variableBlocks.ReplaceText(this.Rhs);
                String lhs = variableBlocks.ReplaceText(this.Lhs);
                return String.Compare(rhs, lhs) == 0;
            }
        }

        public abstract class CStateNum : CState
        {
            public String Rhs;
            protected bool Values(List<VariablesBlock> variableBlocks, out Int32 lhsValue, out Int32 rhsValue)
            {
                lhsValue = -1;
                rhsValue = -1;
                String rhs = variableBlocks.ReplaceText(this.Rhs);
                String lhs = variableBlocks.ReplaceText(this.Lhs);
                if (Int32.TryParse(rhs, out rhsValue) == false)
                    return false;

                if (Int32.TryParse(lhs, out lhsValue) == false)
                    return false;

                return true;
            }
        }

        public class CStateNumEq : CStateNum
        {
            public override bool IsTrue(List<VariablesBlock> variableBlocks)
            {
                if (this.Values(variableBlocks, out Int32 lhsValue, out Int32 rhsValue) == false)
                    return false;
                return lhsValue == rhsValue;
            }
        }

        public class CStateNumLt : CStateNum
        {
            public override bool IsTrue(List<VariablesBlock> variableBlocks)
            {
                if (this.Values(variableBlocks, out Int32 lhsValue, out Int32 rhsValue) == false)
                    return false;
                return lhsValue < rhsValue;
            }
        }

        public class CStateNumLe : CStateNum
        {
            public override bool IsTrue(List<VariablesBlock> variableBlocks)
            {
                if (this.Values(variableBlocks, out Int32 lhsValue, out Int32 rhsValue) == false)
                    return false;
                return lhsValue <= rhsValue;
            }
        }

        public class CStateNumGt : CStateNum
        {
            public override bool IsTrue(List<VariablesBlock> variableBlocks)
            {
                if (this.Values(variableBlocks, out Int32 lhsValue, out Int32 rhsValue) == false)
                    return false;
                return lhsValue > rhsValue;
            }
        }

        public class CStateNumGe : CStateNum
        {
            public override bool IsTrue(List<VariablesBlock> variableBlocks)
            {
                if (this.Values(variableBlocks, out Int32 lhsValue, out Int32 rhsValue) == false)
                    return false;
                return lhsValue >= rhsValue;
            }
        }

        public class Condition
        {
            /// <summary>
            /// Items in this condition
            /// </summary>
            public CState State;

            /// <summary>
            /// Items in this condition
            /// </summary>
            public List<MIBase> Items = new List<MIBase>();
        }

        /// <summary>
        /// Conditions (each if, else if, else ...)
        /// </summary>
        public List<Condition> Conditions = new List<Condition>();

        public MIConditional(String sourceFile,
            Int32 lineNumber) : base(sourceFile, lineNumber)
        {
        }
    }
}
