using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FSHer.Bbl.FSH
{
    public partial class SDEntity
    {
        public class SDPaths
        {
            public SDEntity Container { get; set; }
            public String[] PathValues { get; set; }

            public SDPaths(SDEntity container,
                params String[] pathValues)
            {
                this.Container = container;
                this.PathValues = pathValues;
            }

            public SDPaths Flags(Flags flags)
            {
                FlagRule r = new FlagRule(this.PathValues, flags);
                this.Container.Rules.Add(r);
                return this;
            }
        }
    }
}
