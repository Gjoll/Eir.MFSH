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
        public class Link
        {
            public GraphLink Traversal { get; set; }
            public GraphNode Node { get; set; }
            public Int32 Depth { get; set; }
        }

        public String NodeName { get; set; }
        public String ElementId { get; set; }
        public String DisplayName { get; set; }
        public String CssClass { get; set; }

        public List<Link> ParentLinks { get; } = new List<Link>();
        public List<Link> ChildLinks { get; } = new List<Link>();
        public String LhsAnnotationText { get; set; }
        public String RhsAnnotationText { get; set; }

        public GraphNode(FGrapher fGraph) : base(fGraph)
        {
        }


        public GraphNode(FGrapher fGraph, JToken data) : base(fGraph)
        {
            this.NodeName = this.RequiredValue(data, "nodeName");
            this.DisplayName = this.RequiredValue(data, "displayName");
            this.CssClass = this.OptionalValue(data, "cssClass");
            this.ElementId = this.RequiredValue(data, "elementId");
            this.LhsAnnotationText = this.OptionalValue(data, "lhsAnnotationText");
            this.RhsAnnotationText = this.OptionalValue(data, "rhsAnnotationText");
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
