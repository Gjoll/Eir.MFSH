﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MFSH
{
    public class ApplyInfo
    {
        public String MacroName;
        public String ExpandedText;
        public bool OnceFlag = false;
    }

    public class StackFrame
    {
        public VariablesBlock FrameVariables = new VariablesBlock();
        public List<FileData> Redirections = new List<FileData>();
        public FileData Data;
        public Dictionary<String, ApplyInfo> AppliedMacros = new Dictionary<String, ApplyInfo>();
        public HashSet<String> IncompatibleMacros = new HashSet<String>();

        public StackFrame()
        {
            this.Data = new FileData
            {
                RelativePathType = FileData.RedirType.Text
            };
        }
    }
}
