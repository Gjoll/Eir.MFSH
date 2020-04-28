using System;
using System.Collections.Generic;
using System.Text;

namespace FGraph
{
    class GraphLinkByName
    {
        public String GraphName { get; set; }
        public String Source { get; set; }
        public String Target { get; set; }

        public GraphLinkByName(dynamic data)
        {
            this.GraphName = data.graphName;
            this.Source = data.source;
            this.Target = data.target;
        }
    }
}
