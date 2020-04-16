using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSHer.Bbl.FSH
{
    public class ValueSetRule : SDRule
    {
        public String Units { get; set; }
        public String From { get; set; }
        public Strength Strength{ get; set; }


        public Flags Flags { get; set; }
        
        internal ValueSetRule(String path,
            String from,
            String units = "",
            Strength strength = Strength.None)
        {
            this.Paths.Add(path);
            this.From= from;
            this.Units = units;
            this.Strength = strength;
        }

        public override void WriteFSH(StringBuilder sb)
        {
            sb.WriteLine(2, $"* {this.Path} {this.Units} from {this.From} {StrengthHelper.ToFsh(this.Strength)}");
        }
    }
}
