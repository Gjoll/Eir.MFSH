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
        public enum RedirType
        {
            Json,
            Text
        };

        protected StringBuilder text = new StringBuilder();

        public FileData()
        {
        }

        public virtual void AppendText(String text) => this.text.Append(text);
        public virtual String Text() => this.text.ToString();
        public virtual String SaveText() => this.text.ToString();
        public RedirType RelativePathType { get; set; }
        public String RelativePath { get; set; }

        public void ProcessVariables(VariablesBlock v)
        {
            this.RelativePath = v.ReplaceText(this.RelativePath);
            String text = v.ReplaceText(this.text.ToString());
            this.text.Clear();
            this.text.Append(text);
        }
        public override string ToString()
        {
            if (this.RelativePath != null)
                return this.RelativePath;
            return "No Relative Path";
        }
    }

    public class JsonArrayData: FileData
    {
        public override String SaveText()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[\n");
            sb.Append(this.text.ToString());
            sb.Append("]\n");

            return sb.ToString();
        }
    }
}
