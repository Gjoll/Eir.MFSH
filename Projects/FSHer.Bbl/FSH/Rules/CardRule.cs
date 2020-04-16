using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSHer.Bbl.FSH
{
    public class CardRule : SDRule
    {
        public String Min { get; set; }
        public String Max { get; set; }


        public Flags Flags { get; set; }

        internal CardRule(String path,
            String cardStr,
            Flags flags = Flags.None)
        {
            this.Paths.Add(path);
            this.Card(cardStr);
            this.Flags = flags;
        }

        public CardRule Card(String s)
        {
            Int32 index = s.IndexOf(".");
            if (index > 0)
                this.Min = s.Substring(0, index);

            index = s.LastIndexOf('.');
            if (index < s.Length)
                this.Max = s.Substring(index + 1);

            return this;
        }

        public override void WriteFSH(StringBuilder sb)
        {
            sb.WriteLine(2, $"* {this.Path} {this.Min}..{this.Max} {this.Flags.ToFsh()}");
        }
    }
}
