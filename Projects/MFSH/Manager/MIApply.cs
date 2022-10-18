using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using MFSH;

namespace MFSH
{
    /// <summary>
    /// Apply definition
    /// apply applies a macro.
    /// </summary>
    [DebuggerDisplay("Apply'{Name}'")]
    public class MIApply : MIBase
    {
        /// <summary>
        /// Name of macro to apply.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// True if once specified on call.
        /// If true, then macro is actually only applied once 
        /// (first time called).
        /// </summary>
        public bool OnceFlag { get; set; } = false;

        /// <summary>
        /// Number of times that this apply has been executed.
        /// </summary>
        public Int32 ApplyCount { get; set; } = 0;

        public List<String> Parameters = new List<String>();
        public List<String> Usings = new List<String>();

        public MIApply(String sourceFile,
            Int32 lineNumber) : base(sourceFile, lineNumber)
        {
        }

        public override string ToString()
        {
            const Int32 MaxLen = 64;

            StringBuilder sb = new StringBuilder();
            sb.Append($"Macro: {this.Name}(");
            for (Int32 i = 0; i < this.Parameters.Count; i++)
            {
                String[] parameterLines = this.Parameters[i].Split('\n');
                if (i > 1)
                    sb.Append(",");
                if (parameterLines.Length == 1)
                {
                    if (sb.Length > MaxLen)
                    {
                        sb.AppendLine("");
                        sb.Append("    ");
                    }
                    sb.AppendLine($"\"{parameterLines[0]}\"");
                }
                else
                {
                    sb.AppendLine("");
                    sb.AppendLine("    \"\"\"");
                    for (Int32 j = 0; j < parameterLines.Length; j++)
                        sb.AppendLine($"    {parameterLines[j]}");
                    sb.AppendLine("    \"\"\"");
                }
            }

            sb.AppendLine($")");
            sb.AppendLine($"  Called from '{this.SourceFile}', line {this.LineNumber}");
            return sb.ToString();
        }
    }
}
