using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Hl7.Fhir.Model;
using Newtonsoft.Json.Linq;

namespace FGraph
{
    [DebuggerDisplay("{NodeName}")]
    public class GraphNode : GraphItem
    {
        /// <summary>
        /// Class that defines links to/from this node.
        /// </summary>
        public class Link
        {
            public GraphLink Traversal { get; set; }
            public GraphNode Node { get; set; }
            public Int32 Depth { get; set; }
        }

        /// <summary>
        /// Name of this node.
        /// </summary>
        public String NodeName { get; set; }

        /// <summary>
        /// Optional value of what we linkt his node to. Link can be to
        /// a profile, profile element, value set, code set, etc.
        /// </summary>
        public GraphAnchor Anchor { get; set; }

        /// <summary>
        /// Name to display on graph node. String has multiple parts each seperated
        /// by a '/'. Each part is on its own line.
        /// </summary>
        public String DisplayName { get; set; }

        /// <summary>
        /// css class to set svg element to.
        /// </summary>
        public String CssClass { get; set; }

        public List<Link> ParentLinks { get; } = new List<Link>();
        public List<Link> ChildLinks { get; } = new List<Link>();

        /// <summary>
        /// Left hand side (incoming) annotation text. This is printed on the
        /// line that comes into the graph node.
        /// </summary>
        public String LhsAnnotationText { get; set; }

        /// <summary>
        /// Right hand side (incoming) annotation text. This is printed on the
        /// line that comes into the graph node.
        /// </summary>
        public String RhsAnnotationText { get; set; }

        public GraphNode(FGrapher fGraph) : base(fGraph)
        {
        }


        public GraphNode(FGrapher fGraph, JToken data) : base(fGraph)
        {
            this.NodeName = data.RequiredValue("nodeName");
            this.DisplayName = data.RequiredValue("displayName");
            this.CssClass = data.OptionalValue("cssClass");
            this.LhsAnnotationText = data.OptionalValue("lhsAnnotationText");
            this.RhsAnnotationText = data.OptionalValue("rhsAnnotationText");
            {
                JToken anchor = data["anchor"];
                if (anchor != null)
                    this.Anchor = new GraphAnchor(anchor);
            }
        }

        public void AddChild(GraphLink gLink, Int32 depth, GraphNode child)
        {
            Link link = new Link
            {
                Node = child,
                Traversal = gLink,
                Depth = depth
            };
            this.ChildLinks.Add(link);
        }

        public void AddParent(GraphLink gLink, Int32 depth, GraphNode parent)
        {
            Link link = new Link
            {
                Node = parent,
                Traversal = gLink,
                Depth = depth
            };
            this.ParentLinks.Add(link);
        }
    }
}
