using Eir.DevTools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using System.Drawing;
using FGraph.Extensions;

namespace FGraph
{
    public partial class FGrapher
    {
        public void RenderFocusGraphs(String cssFile)
        {
            foreach (GraphNodeWrapper node in this.graphNodes.Values)
            {
                this.RenderFocusGraph(cssFile, node, $"focus/{node.NodeName.FirstSlashPart()}");
            }
        }

        void RenderFocusGraph(String cssFile,
            GraphNodeWrapper graphNode,
            String traversalName)
        {
            String name = graphNode.NodeName.Replace("/", "-");
            SvgEditor e = new SvgEditor($"FocusGraph_{name}");
            e.AddCssFile(cssFile);

            this.svgEditors.Add(e);
            SENodeGroup seGroupParents = new SENodeGroup("parents", true);
            SENodeGroup seGroupFocus = new SENodeGroup("focus", true);
            SENodeGroup seGroupChildren = new SENodeGroup("children", true);
            seGroupParents.AppendChild(seGroupFocus);
            seGroupFocus.AppendChild(seGroupChildren);

            {
                SENode seNode = this.CreateNode(graphNode, null);
                seNode.Class = "focus";
                seGroupFocus.AppendNode(seNode);
            }

            seGroupParents.AppendNodes(TraverseParents(graphNode, "focus/*"));
            seGroupFocus.AppendChildren(TraverseChildren(graphNode, "focus/*"));

            e.Render(seGroupParents, true);
        }

        protected SENode CreateNode(GraphNodeWrapper graphNode,
            String[] annotations)
        {
            String hRef = null;
            //$if (linkFlag)
            //$    hRef = this.HRef(mapNode);
            SENode node = new SENode(0, annotations, hRef);
            node.Class = graphNode.CssClass;

            String displayName = graphNode.DisplayName;

            foreach (String titlePart in displayName.Split('/'))
            {
                String s = titlePart.Trim();
                node.AddTextLine(s, hRef);
            }
            return node;
        }

        IEnumerable<SENode> TraverseParents(GraphNodeWrapper graphNode,
            String traversalFilter)
        {
            Regex r = new Regex(traversalFilter);

            HashSet<GraphNodeWrapper> parentNodes = new HashSet<GraphNodeWrapper>();
            parentNodes.Add(graphNode);
            graphNode.ParentLinks.SortByTraversalName();

            foreach (GraphNodeWrapper.Link parentLink in graphNode.ParentLinks)
            {
                if (
                    (r.IsMatch(parentLink.Traversal.TraversalName)) &&
                    (parentNodes.Contains(parentLink.Node) == false)
                    )
                {
                    String[] annotations = new String[0];	//$
                    yield return CreateNode(parentLink.Node, annotations);
                }
            }
        }

        IEnumerable<SENodeGroup> TraverseChildren(GraphNodeWrapper graphNode,
            String traversalFilter)
        {
            Regex r = new Regex(traversalFilter);

            HashSet<GraphNodeWrapper> childNodes = new HashSet<GraphNodeWrapper>();
            childNodes.Add(graphNode);
            graphNode.ChildLinks.SortByTraversalName();

            foreach (GraphNodeWrapper.Link childLink in graphNode.ChildLinks)
            {
                if (
                    (r.IsMatch(childLink.Traversal.TraversalName)) &&
                    (childNodes.Contains(childLink.Node) == false)
                )
                {
                    String[] annotations = new String[0];	//$
                    SENodeGroup childContainer = new SENodeGroup("Child", true);
                    SENode child = CreateNode(childLink.Node, annotations);
                    childContainer.AppendNode(child);
                    yield return childContainer;
                }
            }
        }

    }
}
