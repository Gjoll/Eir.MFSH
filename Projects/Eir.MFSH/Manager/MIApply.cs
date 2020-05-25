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
    public class MIApply : MIBase
    {
        public String Name { get; set; }
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
