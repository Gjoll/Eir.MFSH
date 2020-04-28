using System;
using System.Collections.Generic;
using System.Text;

namespace FGraph
{
    public class GraphNode
    {
        public String GraphName { get; set; }
        public String NodeName { get; set; }
        public String DisplayName { get; set; }
        public String CssClass { get; set; }

        public GraphNode(dynamic data)
        {
            this.GraphName = data.graphName;
            this.NodeName = data.nodeName;
            this.DisplayName = data.displayName;
            this.CssClass = data.cssClass;
        }
    }
}
