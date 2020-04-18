using System;
using System.Collections.Generic;
using System.Text;

namespace MFSH
{
    public class ParseInfo
    {
        public StringBuilder ParsedText = new StringBuilder();
    }

    public class DefineInfo : ParseInfo
    {
        public String Name;
        public List<String> Parameters = new List<string>();
    }
}
