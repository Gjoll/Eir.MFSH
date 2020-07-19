using System;
using System.Collections.Generic;
using System.Text;

namespace Eir.MFSH.Manager
{
    public class ApplicableManager
    {
        class Namespace
        {
            public Dictionary<String, Namespace> NameSpaces = new Dictionary<string, Namespace>();
            public Dictionary<String, MIApplicable> Items = new Dictionary<string, MIApplicable>();
        }

        public bool DebugFlag => this.Mfsh.DebugFlag;
        public MFsh Mfsh { get; }

        Namespace baseNamespace;
        Dictionary<String, Namespace> nameSpaces;

        public ApplicableManager(MFsh mfsh)
        {
            this.baseNamespace = new Namespace();
            this.nameSpaces = new Dictionary<string, Namespace>();
            this.nameSpaces.Add("", this.baseNamespace);
            this.Mfsh = mfsh;
        }

        IEnumerable<Namespace> Namespaces(Dictionary<String, Namespace> nsDict)
        {
            foreach (Namespace ns in nsDict.Values)
            {
                yield return ns;
                foreach (Namespace nsChild in this.Namespaces(ns.NameSpaces))
                    yield return nsChild;
            }
        }

        public IEnumerable<MIMacro> Macros()
        {
            foreach (Namespace ns in Namespaces(this.nameSpaces))
            {
                foreach (var item in ns.Items.Values)
                {
                    if (item is MIMacro)
                        yield return item as MIMacro;
                }
            }
        }

        public IEnumerable<MIFragment> Fragments()
        {
            foreach (Namespace ns in Namespaces(this.nameSpaces))
            {
                foreach (var item in ns.Items.Values)
                {
                    if (item is MIFragment)
                        yield return item as MIFragment;
                }
            }
        }

        public bool TryGetItem(String name, out MIApplicable item)
        {
            item = null;
            String[] parts = name.Split('\n');
            Namespace ns = this.baseNamespace;
            for (Int32 i = 0; i < parts.Length - 1; i++)
            {
                if (ns.NameSpaces.TryGetValue(parts[i], out Namespace nsChild) == false)
                    return false;
                ns = nsChild;
            }

            if (ns.Items.TryGetValue(name, out item) == false)
                return false;
            return true;
        }

        public bool TryGetItem(List<String> usings, String name, out MIApplicable item)
        {
            if (TryGetItem(name, out item) == true)
                return true;

            foreach (String use in usings)
            {
                if (TryGetItem($"{use}.{name}", out item) == true)
                    return true;
            }
            return false;
        }

        public bool TryAddItem(String name, MIApplicable item)
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

            if (ns.Items.TryAdd(name, item) == false)
                return false;
            return true;
        }
    }
}
