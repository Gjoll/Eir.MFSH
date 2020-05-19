using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Eir.MFSH;

namespace Eir.MFSH
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
        /// Items in macro
        /// </summary>
        public List<MIBase> Items = new List<MIBase>();

        public MIPreFsh(String sourceFile,
            Int32 lineNumber) : base(sourceFile, lineNumber)
        {
        }

        public String WriteFsh()
        {
            StringBuilder sb = new StringBuilder();

            foreach (MIBase b in this.Items)
            {
                switch (b)
                {
                    case MIText text:
                        sb.Append(text.Line);
                        break;
                }
            }

            return sb.ToString();
        }
    }
}
