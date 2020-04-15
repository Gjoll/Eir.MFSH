using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace FSHer.PreFhir.FSH
{
    [DebuggerDisplay("Alias: {Name} = {Value}")]
    public class Alias : Entity
    {
        public String Name { get; set; }
        public String Value { get; set; }
        public override void WriteFSH(StringBuilder sb)
        {
            base.WriteFSH(sb);
            WriteLine(sb, 1, $"  Alias: {this.Name} = {this.Value}");
        }
    }
}
