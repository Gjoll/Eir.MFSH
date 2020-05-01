using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace FGraph
{
    class GraphLinkByNameWrapper: GraphLinkWrapper
    {
        public String Source { get; set; }
        public String Target { get; set; }

        public GraphLinkByNameWrapper(JToken data) : base(data)
        {
            this.Source = this.RequiredValue(data, "source");
            this.Target = this.RequiredValue(data, "target");
        }
    }
}
