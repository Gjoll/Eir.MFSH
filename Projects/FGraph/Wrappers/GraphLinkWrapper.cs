using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FGraph
{
    public abstract class GraphLinkWrapper : GraphWrapper
    {
        public String TraversalName { get; set; }
        public String SourceText { get; set; }
        public String TargetText { get; set; }

        public GraphLinkWrapper(JToken data) : base(data)
        {
            this.TraversalName = this.RequiredValue(data, "traversalName");
            this.SourceText = this.OptionalValue(data, "sourceText");
            this.TargetText = this.OptionalValue(data, "targetText");
        }
    }
}
