using Eir.DevTools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MFSH
{
    [DebuggerDisplay("{ToString()}")]
    public class FileData
    {
        protected StringBuilder text = new StringBuilder();

        public virtual void AppendText(String text) => this.text.Append(text);
        public virtual String GetText() => this.text.ToString();
        public String RelativePath { get; set; }

        public override string ToString()
        {
            if (this.RelativePath != null)
                return this.RelativePath;
            return "No Relative Path";
        }
    }

    public class DefineInfo : FileData
    {
        public String RedirectDataPath { get; set; }
        public String Name;
        public List<String> Parameters = new List<string>();
    }

    public class JsonArrayData: FileData
    {
        public override String GetText()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[\n");
            sb.Append(this.text.ToString());
            sb.Append("]\n");

            return sb.ToString();
        }
    }
}
