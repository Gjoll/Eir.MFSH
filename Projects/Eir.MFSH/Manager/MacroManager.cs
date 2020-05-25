using System;
using System.Collections.Generic;
using System.Text;

namespace Eir.MFSH.Manager
{
    public class MacroManager
    {
        class Namespace
        {
            public Dictionary<String, Namespace> NameSpaces = new Dictionary<string, Namespace>();
            public Dictionary<String, MIMacro> Macros = new Dictionary<string, MIMacro>();
        }

        public bool DebugFlag => this.Mfsh.DebugFlag;
        public MFsh Mfsh { get; }

        Namespace baseNamespace;
        Dictionary<String, Namespace> nameSpaces;

        public MacroManager(MFsh mfsh)
        {
            this.baseNamespace = new Namespace();
            this.nameSpaces = new Dictionary<string, Namespace>();
            this.nameSpaces.Add("", this.baseNamespace);
            this.Mfsh = mfsh;
        }

        bool TryGetMacro(String name, out MIMacro macro)
        {
            macro = null;
            String[] parts = name.Split('\n');
            Namespace ns = this.baseNamespace;
            for (Int32 i = 0; i < parts.Length - 1; i++)
            {
                if (ns.NameSpaces.TryGetValue(parts[i], out Namespace nsChild) == false)
                    return false;
                ns = nsChild;
            }

            if (ns.Macros.TryGetValue(name, out macro) == false)
                return false;
            return true;
        }

        public bool TryGetMacro(List<VariablesBlock> variablesBlock, String name, out MIMacro macro)
        {
            if (TryGetMacro(name, out macro) == true)
                return true;
            foreach (VariablesBlock variableBlock in variablesBlock)
            {
                foreach (String use in variableBlock.Usings)
                {
                    if (TryGetMacro($"{use}.{name}", out macro) == true)
                        return true;
                }
            }
            return false;
        }

        public bool TryAddMacro(String name, MIMacro macro)
        {
            String[] parts = name.Split('\n');
            Namespace ns = this.baseNamespace;
            for (Int32 i = 0; i < parts.Length - 1; i++)
            {
                if (ns.NameSpaces.TryGetValue(parts[i], out Namespace nsChild) == false)
                {
                    nsChild = new Namespace();
                    ns.NameSpaces.Add(parts[i], nsChild);
                }
                ns = nsChild;
            }

            if (ns.Macros.TryAdd(name, macro) == false)
                return false;
            return true;
        }
    }
}
