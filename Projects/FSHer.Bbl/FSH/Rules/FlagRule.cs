using System;
using System.Collections.Generic;
using System.Text;

namespace FSHer.Bbl.FSH
{
    public class FlagRule : SDRule
    {
        public Flags Flags { get; set; } = Flags.None;

        internal FlagRule(String path,
            Flags flags)
        {
            this.Paths.Add(path);
            this.Flags = flags;
        }

        internal FlagRule(IEnumerable<String> paths,
            Flags flags)
        {
            this.Paths.AddRange(paths);
            this.Flags = flags;
        }

        public override void WriteFSH(StringBuilder sb)
        {
            sb
                .WriteMargin(2)
                .Write("* ")
                .WritePaths(this.Paths)
                .WriteLine(this.Flags.ToFsh())
                ;
        }
    }
}
