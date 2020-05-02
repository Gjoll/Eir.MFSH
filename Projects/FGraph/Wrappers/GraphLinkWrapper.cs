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
        public Int32 Depth { get; set; }

        public GraphLinkWrapper(FGrapher fGraph, JToken data) : base(fGraph, data)
        {
            this.TraversalName = this.RequiredValue(data, "traversalName");
            this.Depth = this.OptionalIntValue(data, "depth", 1);
        }
    }
}
