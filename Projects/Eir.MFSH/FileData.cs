using Eir.DevTools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Eir.MFSH
{
    [DebuggerDisplay("{" + nameof(FileData.RelativePath) + "}")]
    public class FileData
    {
        public String RelativePath { get; set; }
        public StringBuilder Text { get; }  = new StringBuilder();

        public FileData()
        {
        }
    }
}
