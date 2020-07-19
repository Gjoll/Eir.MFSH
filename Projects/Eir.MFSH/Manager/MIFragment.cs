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
    [DebuggerDisplay("Frag '{Name}'")]
    public class MIFragment : MIApplicable
    {
        /// <summary>
        /// Parent fhir class
        /// </summary>
        public String Parent { get; set; } = null;

        /// <summary>
        /// Fhir title
        /// </summary>
        public String Title { get; set; } = null;

        /// <summary>
        /// Fhir description
        /// </summary>
        public String Description { get; set; } = null;

        /// <summary>
        /// If set text definition of fragment
        /// </summary>
        public String FragmentDefinition { get; set; } = null;

        public MIFragment(String sourceFile,
            Int32 lineNumber,
            String fragName) : base(sourceFile, lineNumber, fragName)
        {
            String fragRPath = sourceFile;
            String fragFileNameNoExtension = Path.GetFileNameWithoutExtension(fragRPath);
            String fragFileName = Path.GetFileName(fragRPath);
            String fragDir = Path.GetDirectoryName(fragRPath);

            this.ApplicableVariables.Set("%FragName%", fragName);
            this.ApplicableVariables.Set("%FragPath%", fragRPath);
            this.ApplicableVariables.Set("%FragDir%", fragDir);
            this.ApplicableVariables.Set("%FragFileName%", fragFileName);
            this.ApplicableVariables.Set("%FragFileNameNoExtension%", fragFileNameNoExtension);
        }
    }
}
