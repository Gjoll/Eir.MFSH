using Eir.DevTools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Eir.MFSH
{
    [DebuggerDisplay("{" + nameof(FileData.AbsoluteOutputPath) + "}")]
    public class FileData
    {
        StringBuilder txtBuilder = new StringBuilder();

        public String AbsoluteOutputPath { get; set; }

        public String Text => this.txtBuilder.ToString();

        public FileData()
        {
        }

        public void AppendLine(String text)
        {
            //Debug.Assert(text.Contains('%') == false);
            this.txtBuilder.AppendLine(text);
        }

        public void Append(String text)
        {
            //Debug.Assert(text.Contains('%') == false);
            this.txtBuilder.Append(text);
        }
    }
}
