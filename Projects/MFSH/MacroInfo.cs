using System;
using System.Collections.Generic;
using System.Text;

namespace MFSH
{
    public class MacroInfo
    {
        public String Name;
        public List<String> Parameters = new List<string>();
        public StringBuilder ParsedText = new StringBuilder();
    }
}
