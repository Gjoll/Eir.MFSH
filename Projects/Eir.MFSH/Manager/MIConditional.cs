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
