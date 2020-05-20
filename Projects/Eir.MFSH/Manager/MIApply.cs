﻿using System;
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

        public MIApply(String sourceFile,
            Int32 lineNumber) : base (sourceFile, lineNumber)
        {
        }
    }
}
