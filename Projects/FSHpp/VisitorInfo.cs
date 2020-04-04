using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp
{
    public class VisitorInfo
    {
        public StringBuilder Output = new StringBuilder();
        public String Input;
        public Int32 InputIndex = 0;


        public void CopyToEnd()
        {
            CopyBytes(this.Input.Length);
        }

        public void SkipBytes(Int32 newIndex)
        {
            this.InputIndex = newIndex;
        }
        public void CopyBytes(Int32 newIndex)
        {
            this.Output.Append((this.Input.Substring(this.InputIndex,
                newIndex - this.InputIndex)));
            SkipBytes(newIndex);
        }
    }
}
