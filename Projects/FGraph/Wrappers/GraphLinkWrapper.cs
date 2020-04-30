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
        public Color BackgroundColor { get; set; }

        public GraphLinkWrapper(JToken data) : base(data)
        {
            this.TraversalName = this.RequiredValue("GraphLinkByName.traversalName", data["traversalName"]);
            this.BackgroundColor = this.OptionalColorValue("GraphLinkByName.backgroundColor", data["backgroundColor"]);
        }

    }
}
