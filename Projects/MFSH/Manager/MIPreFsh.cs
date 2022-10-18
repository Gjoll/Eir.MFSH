using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using MFSH;

namespace MFSH
{
    /// <summary>
    /// This file contains items that will become a fsh file.
    /// </summary>
    [DebuggerDisplay("PreFsh {RelativePath}")]
    public class MIPreFsh : MIBase
    {
        /// <summary>
        /// Output relative path
        /// </summary>
        public String RelativePath { get; set; }

        /// <summary>
        /// Usings defined in this file.
        /// </summary>
        public List<String> Usings = new List<String>();

        /// <summary>
        /// Items in macro
        /// </summary>
        public List<MIBase> Items = new List<MIBase>();

        public MIPreFsh(String sourceFile,
            Int32 lineNumber) : base(sourceFile, lineNumber)
        {
        }
    }
}
