using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSHer.Bbl.FSH
{
    public abstract class  SDRule : Item
    {
        public String Path => Paths.First();
        public List<String> Paths { get; set; } = new List<string>();
    }
}
