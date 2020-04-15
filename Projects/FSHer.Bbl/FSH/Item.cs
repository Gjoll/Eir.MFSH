using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace FSHer.Bbl.FSH
{
    public abstract class Item
    {
        public String Documentation { get; set; }

        public virtual void WriteFSH(StringBuilder sb)
        {
            String doc = this.Documentation.Replace("\r", "");
            foreach (String line in doc.Split('\n'))
                sb.WriteLine(1, $"  // {line}");
        }
    }
}
