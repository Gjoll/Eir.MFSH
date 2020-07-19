using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Text;
using Eir.MFSH;

namespace Eir.MFSH
{
    /// <summary>
    /// Base class for MIMacro and MIFrag
    /// </summary>
    [DebuggerDisplay("{Name}")]
    public class MIApplicable : MIBase
    {
        /// <summary>
        /// Name of macro
        /// </summary>
        public String Name { get; }

        /// <summary>
        /// Variables that are local to this macro
        /// </summary>
        public VariablesBlock ApplicableVariables { get; } = new VariablesBlock();

        /// <summary>
        /// Items in macro
        /// </summary>
        public List<MIBase> Items = new List<MIBase>();
        public MIApplicable(String sourceFile,
            Int32 lineNumber,
            String macroName) : base(sourceFile, lineNumber)
        {
            this.Name = macroName;
            String macroRPath = sourceFile;
            String macroFileNameNoExtension = Path.GetFileNameWithoutExtension(macroRPath);
            String macroFileName = Path.GetFileName(macroRPath);
            String macroDir = Path.GetDirectoryName(macroRPath);
        }
    }
}
