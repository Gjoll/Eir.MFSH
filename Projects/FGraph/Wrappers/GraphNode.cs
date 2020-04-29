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
            public String TraversalName { get; set; }
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

        public void AddChild(String traversalName, GraphNode child)
        {
            Link link = new Link
            {
                Node = child,
                TraversalName = traversalName
            };
            this.ChildLinks.Add(link);
        }

        public void AddParent(String traversalName, GraphNode parent)
        {
            Link link = new Link
            {
                Node = parent,
                TraversalName = traversalName
            };
            this.ParentLinks.Add(link);
        }
    }
}
