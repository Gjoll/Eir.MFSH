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
    /// Macro definition
    /// </summary>
    [DebuggerDisplay("Macro '{Name}'")]
    public class MIMacro : MIApplicable
    {
        /// <summary>
        /// If set, base output dir for fragment definition
        /// </summary>
        public String FragmentBase { get; set; } = null;

        /// <summary>
        /// If set text definition of fragment
        /// </summary>
        public String FragmentDefinition { get; set; } = null;

        /// <summary>
        /// If set, this macro has been applied.
        /// </summary>
        public bool AppliedFlag { get; set; } = false;

        /// <summary>
        /// If set, this macro is only instantiated once.
        /// </summary>
        public bool SingleFlag { get; set; }

        /// <summary>
        /// Output path if redirection set
        /// </summary>
        public String Redirect { get; set; }

        /// <summary>
        /// Macro parameters
        /// </summary>
        public List<String> Parameters { get; } = new List<String>();

        /// <summary>
        /// Current use'ings
        /// </summary>
        public List<String> Usings = new List<String>();

        public MIMacro(String sourceFile,
            Int32 lineNumber,
            String macroName,
            IEnumerable<String> parameters) : base(sourceFile, lineNumber, macroName)
        {
            this.Parameters.AddRange(parameters);
            String macroRPath = sourceFile;
            String macroFileNameNoExtension = Path.GetFileNameWithoutExtension(macroRPath);
            String macroFileName = Path.GetFileName(macroRPath);
            String macroDir = Path.GetDirectoryName(macroRPath);

            this.ApplicableVariables.Set("%MacroName%", macroName);
            this.ApplicableVariables.Set("%MacroPath%", macroRPath);
            this.ApplicableVariables.Set("%MacroDir%", macroDir);
            this.ApplicableVariables.Set("%MacroFileName%", macroFileName);
            this.ApplicableVariables.Set("%MacroFileNameNoExtension%", macroFileNameNoExtension);
        }
    }
}
