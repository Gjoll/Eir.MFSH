using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FGraph
{
    public abstract class GraphLink : GraphItem
    {
        public String TraversalName { get; set; }
        public Int32 Depth { get; set; }

        public GraphLink(FGrapher fGraph, JToken data) : base(fGraph)
        {
            this.TraversalName = data.RequiredValue("traversalName");
            this.Depth = data.OptionalIntValue("depth", 1);
        }
    }
}
