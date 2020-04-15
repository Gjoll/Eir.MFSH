using System;
using System.Collections.Generic;
using System.Text;

namespace FSHer.Bbl.FSH
{
    public interface ICardRuleContainer : TIEntity<ICardRuleContainer>
    {
    }


    public class CardRule : SDRule
    {
        public Int32 Min { get; set; } = -1;
        public Int32 Max { get; set; } = -1;

        public Flags Flags { get; set; }
        public override void WriteFSH(StringBuilder sb)
        {
            StringBuilder sbFlag = new StringBuilder();
            if ((this.Flags & Flags.MOD) == Flags.MOD)
                sbFlag.Append(" MOD");
            if ((this.Flags & Flags.MS) == Flags.MS)
                sbFlag.Append(" MS");
            if ((this.Flags & Flags.SU) == Flags.SU)
                sbFlag.Append(" SU");
            if ((this.Flags & Flags.TU) == Flags.TU)
                sbFlag.Append(" TU");
            if ((this.Flags & Flags.NORMATIVE) == Flags.NORMATIVE)
                sbFlag.Append(" NORMATIVE");
            if ((this.Flags & Flags.DRAFT) == Flags.DRAFT)
                sbFlag.Append(" DRAFT");

            String min = Min >= 0 ? Min.ToString() : "";
            String max = Max >= 0 ? Max.ToString() : "";
            sb.WriteLine(2, $"* {this.Path.Value} {min}..{max} {sbFlag.ToString()}");
        }
    }

    public static class CardRuleExtensions
    {
        public static void Cardinality(this ICardRuleContainer container,
            String path,
            Int32 min = 0,
            Int32 max = Int32.MaxValue)
        {
            CardRule c = new CardRule()
            {
                Min = min,
                Max = max
            };

            container.Rules.Add(c);
        }
    }
}
