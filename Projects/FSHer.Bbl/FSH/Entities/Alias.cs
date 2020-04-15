using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using FSHer.Bbl.FSH;

namespace FSHer.Bbl.FSH
{
    [DebuggerDisplay("Alias: {Name} = {Value}")]
    public class Alias : Entity
    {
        public String Name { get; set; }
        public String Value { get; set; }
        public override void WriteFSH(StringBuilder sb)
        {
            base.WriteFSH(sb);
            sb.WriteLine(1, $"  Alias: {this.Name} = {this.Value}");
        }
    }
}
