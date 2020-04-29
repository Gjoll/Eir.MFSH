using System;
using System.Collections.Generic;
using System.Text;

namespace FGraph
{
    public class GraphNode
    {
        public class Link
        {
            public GraphNode Node { get; set; }
        }

        public String GraphName { get; set; }
        public String NodeName { get; set; }
        public String DisplayName { get; set; }
        public String CssClass { get; set; }

        public List<Link> ParentLinks { get; } = new List<Link>();
        public List<Link> ChildLinks { get; } = new List<Link>();

        public GraphNode(dynamic data)
        {
            this.GraphName = data.graphName;
            this.NodeName = data.nodeName;
            this.DisplayName = data.displayName;
            this.CssClass = data.cssClass;
        }
    }
}
