using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace FGraph
{
    class GraphLinkByName: GraphLink
    {
        public String Source { get; set; }
        public String Target { get; set; }

        public GraphLinkByName(FGrapher fGraph, JToken data) : base(fGraph, data)
        {
            this.Source = data.RequiredValue("source");
            this.Target = data.RequiredValue("target");
        }
    }
}
