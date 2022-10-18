using Eir.DevTools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace MFSH
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
            this.txtBuilder.AppendLine(text);
        }

        public void Append(String text)
        {
            this.txtBuilder.Append(text);
        }
    }
}
