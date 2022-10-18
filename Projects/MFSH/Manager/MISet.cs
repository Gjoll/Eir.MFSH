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
    public class MISet : MIBase
    {
        /// <summary>
        /// Name of set variable
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Value of set variable
        /// </summary>
        public String Value { get; set; }
        public MISet(String sourceFile,
            Int32 lineNumber) : base(sourceFile, lineNumber)
        {
        }

    }
}
