using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FSHer.Bbl.FSH
{
    /// <summary>
    /// Parent class of either Profile or Extension.
    /// </summary>
    [DebuggerDisplay("{EntityName}: {Name}")]
    public abstract partial class SDEntity : Entity,
        IParent,
        IId,
        ITitle,
        IDescription,
        IMixins
    {
        public String EntityName { get; }
        public String Name { get; set; }
        public String Parent { get; set; }
        public String Id { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public IEnumerable<String> Mixins { get; set; } = new List<string>();

        public SDEntity(String entityName)
        {
            this.EntityName = entityName;
        }

        public SDPath Path(String path) => new SDPath(this, path);
        public SDEntity CaretValue(String caretPath,
            String value)
        {
            CaretValueRule c = new CaretValueRule(null, caretPath, value);
            this.Rules.Add(c);
            return this;
        }

        public override void WriteFSH(StringBuilder sb)
        {
            base.WriteFSH(sb);
            sb
                .WriteLine(1, $"{EntityName}: {this.Name}")
                .WriteTitle(this)
                .WriteParent(this)
                .WriteId(this)
                .WriteDescription(this)
                .WriteMixins(this)
                ;
        }
    }
}
