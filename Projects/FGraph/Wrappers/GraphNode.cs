using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace FGraph
{
    [DebuggerDisplay("{NodeName}")]
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

        public GraphNode()
        {
        }

        public GraphNode(dynamic data)
        {
            this.GraphName = data.graphName;
            this.NodeName = data.nodeName;
            this.DisplayName = data.displayName;
            this.CssClass = data.cssClass;
        }

        public void AddChild(GraphNode child)
        {
            Link link = new Link
            {
                Node = child
            };
            this.ChildLinks.Add(link);
        }

        public void AddParent(GraphNode parent)
        {
            Link link = new Link
            {
                Node = parent
            };
            this.ParentLinks.Add(link);
        }
    }
}
