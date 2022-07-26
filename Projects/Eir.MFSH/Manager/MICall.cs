using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Eir.MFSH;

namespace Eir.MFSH
{
    /// <summary>
    /// Macro definition
    /// </summary>
    [DebuggerDisplay("Incompatible '{Name}'")]
    public class MICall : MIBase
    {
        /// <summary>
        /// Name of set variable
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Parameters to pass to external command
        /// </summary>
        public List<String> Parameters { get; set; } = new List<String>();

        public MICall(String sourceFile,
            Int32 lineNumber) : base(sourceFile, lineNumber)
        {
        }

    }
}
