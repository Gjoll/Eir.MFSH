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
    public class MIMacro : MIBase
    {
        /// <summary>
        /// Variables that are local to this macro
        /// </summary>
        public VariablesBlock MacroVariables { get; } = new VariablesBlock();

        /// <summary>
        /// If set, this macro is only instantiated once per profile.
        /// </summary>
        public bool OnceFlag { get; set; }

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
        /// Name of macro
        /// </summary>
        public String Name { get; }

        /// <summary>
        /// Macro parameters
        /// </summary>
        public List<String> Parameters { get; } = new List<String>();

        /// <summary>
        /// Current use'ings
        /// </summary>
        public List<String> Usings = new List<String>();

        /// <summary>
        /// Items in macro
        /// </summary>
        public List<MIBase> Items = new List<MIBase>();
        public MIMacro(String sourceFile,
            Int32 lineNumber,
            String macroName,
            IEnumerable<String> parameters) : base(sourceFile, lineNumber)
        {
            this.Name = macroName;
            this.Parameters.AddRange(parameters);
            String macroRPath = sourceFile;
            String macroFileNameNoExtension = Path.GetFileNameWithoutExtension(macroRPath);
            String macroFileName = Path.GetFileName(macroRPath);
            String macroDir = Path.GetDirectoryName(macroRPath);

            this.MacroVariables.Set("%MacroName%", macroName);
            this.MacroVariables.Set("%MacroPath%", macroRPath);
            this.MacroVariables.Set("%MacroDir%", macroDir);
            this.MacroVariables.Set("%MacroFileName%", macroFileName);
            this.MacroVariables.Set("%MacroFileNameNoExtension%", macroFileNameNoExtension);
        }
    }
}
