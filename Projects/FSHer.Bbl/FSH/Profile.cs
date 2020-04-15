using System;
using System.Collections.Generic;
using System.Text;

namespace FSHer.PreFhir.FSH
{
    public class Profile : Entity
    {
        public String Name { get; set; }
        public String Value { get; set; }
        public override void WriteFSH(StringBuilder sb)
        {
            base.WriteFSH(sb);
            WriteLine(sb, 1, $"  Profile: {this.Name}");
        }
    }
}
