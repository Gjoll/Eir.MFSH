using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using MFSH;

namespace MFSH
{
    /// <summary>
    /// Macro definition
    /// </summary>
    [DebuggerDisplay("Incompatible '{Name}'")]
    public class MIIncompatible : MIBase
    {
        /// <summary>
        /// Name of Incompatible macro
        /// </summary>
        public String Name { get; set; }
        public MIIncompatible(String sourceFile,
            Int32 lineNumber) : base(sourceFile, lineNumber)
        {
        }

    }
}
