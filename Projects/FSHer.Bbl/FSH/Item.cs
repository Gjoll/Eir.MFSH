using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace FSHer.PreFhir.FSH
{
    public abstract class Item
    {
        public String Documentation { get; set; }

        protected void WriteLine(StringBuilder sb, Int32 margin, String s)
        {
            for (Int32 i = 0; i < margin; i++)
                sb.Append("  ");
            sb.AppendLine(s);
        }


        public virtual void WriteFSH(StringBuilder sb)
        {
            String doc = this.Documentation.Replace("\r", "");
            foreach (String line in doc.Split('\n'))
                WriteLine(sb, 1, $"  // {line}");
        }
    }
}
