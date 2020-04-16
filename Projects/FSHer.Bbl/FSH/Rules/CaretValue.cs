using System;
using System.Collections.Generic;
using System.Text;

namespace FSHer.Bbl.FSH
{
    public class CaretValueRule : SDRule
    {
        public String CaretPath { get; set; }
        public String Value { get; set; }

        internal CaretValueRule(String path,
            String caretPath,
            String value)
        {
            this.Paths.Add(path);
            this.CaretPath= caretPath;
            this.Value= value;
        }

        public override void WriteFSH(StringBuilder sb)
        {
            sb.WriteLine(2, $"* {this.Path} {this.CaretPath} = {this.Value}");
        }
    }
}
