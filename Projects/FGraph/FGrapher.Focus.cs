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

            SENode seNode = this.CreateNode(graphNode);
            seNode.Class = "focus";
            seGroupFocus.AppendNode(seNode);

            seGroupParents.AppendNodes(TraverseParents(graphNode, seNode, "focus/*"));
            seGroupFocus.AppendChildren(TraverseChildren(graphNode, seNode, "focus/*"));

            e.Render(seGroupParents, true);
        }

        protected SENode CreateNode(GraphNodeWrapper graphNode)
        {
            String hRef = null;
            //$if (linkFlag)
            //$    hRef = this.HRef(mapNode);
            SENode node = new SENode
            {
                HRef = hRef
            };
            node.Class = graphNode.CssClass;

            String displayName = graphNode.DisplayName;

            foreach (String titlePart in displayName.Split('/'))
            {
                String s = titlePart.Trim();
                node.AddTextLine(s, hRef);
            }
            return node;
        }

        String ResolveAnnotation(GraphNodeWrapper node,
            GraphNodeWrapper.Link link,
            String annotationSource)
        {
            if (String.IsNullOrEmpty(annotationSource))
                return annotationSource;
 
            return null;
        }

        void SetLhsAnnotation(SENode node, String text)
        {
            //text = "lhs";
            if (String.IsNullOrEmpty(text))
                return;

            if (String.IsNullOrEmpty(node.LhsAnnotation) == false)
            {
                if (String.Compare(node.LhsAnnotation, text, StringComparison.InvariantCulture) != 0)
                {
                    this.ConversionWarn("FGrapher",
                        "SetLHSAnnotation",
                        $"Node {node.AllText()}. LHS Annotation '{node.LhsAnnotation}' overwritten by '{text}");
                }
            }
            node.LhsAnnotation = text;
        }

        void SetRhsAnnotation(SENode node, String text)
        {
            //text = "rhs";
            if (String.IsNullOrEmpty(text))
                return;

            if (String.IsNullOrEmpty(node.RhsAnnotation) == false)
            {
                if (String.Compare(node.RhsAnnotation, text, StringComparison.InvariantCulture) != 0)
                {
                    this.ConversionWarn("FGrapher",
                        "SetRhsAnnotation",
                        $"Node {node.AllText()}. RHS Annotation '{node.RhsAnnotation}' overwritten by '{text}");
                }
            }
            node.RhsAnnotation = text;
        }

        IEnumerable<SENode> TraverseParents(GraphNodeWrapper focusNode,
            SENode seFocusNode,
            String traversalFilter)
        {
            Regex r = new Regex(traversalFilter);

            HashSet<GraphNodeWrapper> parentNodes = new HashSet<GraphNodeWrapper>();
            parentNodes.Add(focusNode);
            focusNode.ParentLinks.SortByTraversalName();

            foreach (GraphNodeWrapper.Link parentLink in focusNode.ParentLinks)
            {
                if (
                    (r.IsMatch(parentLink.Traversal.TraversalName)) &&
                    (parentNodes.Contains(parentLink.Node) == false)
                    )
                {
                    String parentAnnotation = ResolveAnnotation(parentLink.Node, parentLink, parentLink.Annotation);
                    String focusAnnotation = ResolveAnnotation(focusNode, parentLink, parentLink.Annotation);

                    SENode parent = CreateNode(parentLink.Node);
                    this.SetLhsAnnotation(seFocusNode, focusAnnotation);
                    this.SetRhsAnnotation(parent, parentAnnotation);
                    yield return parent;
                }
            }
        }

        IEnumerable<SENodeGroup> TraverseChildren(GraphNodeWrapper focusNode,
            SENode seFocusNode,
            String traversalFilter)
        {
            Regex r = new Regex(traversalFilter);

            HashSet<GraphNodeWrapper> childNodes = new HashSet<GraphNodeWrapper>();
            childNodes.Add(focusNode);
            focusNode.ChildLinks.SortByTraversalName();

            foreach (GraphNodeWrapper.Link childLink in focusNode.ChildLinks)
            {
                if (
                    (r.IsMatch(childLink.Traversal.TraversalName)) &&
                    (childNodes.Contains(childLink.Node) == false)
                )
                {
                    String focusAnnotation = ResolveAnnotation(focusNode, childLink, childLink.Annotation);
                    String childAnnotation = ResolveAnnotation(childLink.Node, childLink, childLink.Annotation);

                    SENodeGroup childContainer = new SENodeGroup("Child", true);
                    SENode child = CreateNode(childLink.Node);
                    this.SetRhsAnnotation(seFocusNode, focusAnnotation);
                    this.SetLhsAnnotation(child, childAnnotation);
                    childContainer.AppendNode(child);
                    yield return childContainer;
                }
            }
        }

    }
}
