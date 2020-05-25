using System;
using System.Collections.Generic;
using System.Text;

namespace Eir.MFSH.Manager
{
    public class MacroManager
    {
        public bool DebugFlag => this.Mfsh.DebugFlag;
        public MFsh Mfsh { get; }

        Dictionary<String, MIMacro> Macros = new Dictionary<string, MIMacro>();
        public bool TryGetMacro(String name, out MIMacro block) => this.Macros.TryGetValue(name, out block);
        public bool TryAddMacro(String name, MIMacro macro) => this.Macros.TryAdd(name, macro);

        public MacroManager(MFsh mfsh)
        {
            this.Mfsh = mfsh;
        }
    }
}
