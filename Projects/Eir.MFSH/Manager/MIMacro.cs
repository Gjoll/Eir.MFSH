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
    [DebuggerDisplay("Macro '{Name}'")]
    public class MIMacro : MIBase
    {
        /// <summary>
        /// Output path if redirection set
        /// </summary>
        public String Redirect { get; set; }

        /// <summary>
        /// Name of macro
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Macro parameters
        /// </summary>
        public List<String> Parameters = new List<String>();

        /// <summary>
        /// Items in macro
        /// </summary>
        public List<MIBase> Items = new List<MIBase>();
        public MIMacro(String sourceFile,
            Int32 lineNumber) : base(sourceFile, lineNumber)
        {
        }
    }
}
