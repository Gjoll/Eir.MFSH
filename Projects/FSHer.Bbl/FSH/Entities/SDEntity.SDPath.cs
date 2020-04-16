using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FSHer.Bbl.FSH
{
    public partial class SDEntity
    {
        public class SDPath
        {
            public SDEntity Container { get; set; }
            public String PathValue { get; set; }

            public SDPath(SDEntity container,
                String pathValue)
            {
                this.Container = container;
                this.PathValue = pathValue;
            }

            public CardRule Card(String card,
                Flags flags = FSH.Flags.None)
            {
                CardRule rule = new CardRule(this.PathValue, card, flags);
                this.Container.Rules.Add(rule);
                return rule;
            }

            public SDPath Flags(Flags flags = FSH.Flags.None)
            {
                FlagRule r = new FlagRule(this.PathValue, flags);
                this.Container.Rules.Add(r);
                return this;
            }

            public SDPath ValueSet(String from,
                String units = "",
                Strength strength = Strength.None)
            {
                ValueSetRule c = new ValueSetRule(this.PathValue, from, units, strength);
                this.Container.Rules.Add(c);
                return this;
            }

            public SDPath CaretValue(String caretPath,
                String value)
            {
                CaretValueRule c = new CaretValueRule(this.PathValue, caretPath, value);
                this.Container.Rules.Add(c);
                return this;
            }

        }
    }
}
