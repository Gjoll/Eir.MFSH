using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MFSH
{
    public class ParseInfo
    {
        public StringBuilder ParsedText = new StringBuilder();
    }

    public class DefineInfo : ParseInfo
    {
        public FileData RedirectData { get; set; }
        public String Name;
        public List<String> Parameters = new List<string>();
    }
}
