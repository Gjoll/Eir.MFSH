using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Eir.MFSH;

namespace Eir.MFSH
{
    /// <summary>
    /// Apply definition
    /// apply applies a macro.
    /// </summary>
    [DebuggerDisplay("Apply'{Name}'")]
    public class MIUse : MIBase
    {
        public String Name { get; set; }

        public MIUse(String sourceFile,
            Int32 lineNumber) : base(sourceFile, lineNumber)
        {
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Use: {this.Name}(");
            sb.AppendLine($")");
            sb.AppendLine($"  Called from '{this.SourceFile}', line {this.LineNumber}");
            return sb.ToString();
        }
    }
}
