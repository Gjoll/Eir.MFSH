using System;
using System.Collections.Generic;
using System.Text;

namespace FSHer.PreFhir.FSH
{
    /// <summary>
    /// FSH document
    /// </summary>
    public class Document : Item
    {
        public String Path;
        public List<Entity> Entities = new List<Entity>();
        public override void WriteFSH(StringBuilder sb)
        {
            throw new NotImplementedException();
        }
    }
}
