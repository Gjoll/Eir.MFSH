using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSHer.PreFhir.FSH
{
    /// <summary>
    /// Parent class of either Profile or Extension.
    /// </summary>
    public abstract class SDEntity : Entity
    {
        public String Name { get; set; }
        public String Parent { get; set; }
        public String Id { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public IEnumerable<String> Mixins { get; set; } = new List<string>();

        protected void WriteFSH(String entityName, StringBuilder sb)
        {
            base.WriteFSH(sb);
            WriteLine(sb, 1, $"{entityName}: {this.Name}");
            if (String.IsNullOrEmpty(this.Parent))
                WriteLine(sb, 1, $"Parent: {this.Parent}");
            if (String.IsNullOrEmpty(this.Id))
                WriteLine(sb, 1, $"Id: {this.Id}");
            if (String.IsNullOrEmpty(this.Title))
                WriteLine(sb, 1, $"Title: {this.Title}");
            if (String.IsNullOrEmpty(this.Description))
                WriteLine(sb, 1, $"Description: {this.Description}");
            if ((this.Mixins != null) && (this.Mixins.Count() > 0))
            {
                String[] mixins = this.Mixins.ToArray();

                String comma = mixins.Length == 1 ? "" : ",";
                WriteLine(sb, 1, $"Mixins: {this.Mixins.First()}{comma}");
                for (Int32 i = 1; i < mixins.Length-1; i++)
                    WriteLine(sb, 2, $"{mixins[i]},");
                WriteLine(sb, 2, $"{mixins.Last()}");
            }
        }
    }
}
