﻿using Eir.DevTools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using System.Drawing;
using FGraph.Extensions;
using Hl7.Fhir.Model;

namespace FGraph
{
    public partial class FGrapher
    {
        public void RenderFocusGraphs(String cssFile)
        {
            foreach (GraphNodeWrapper node in this.graphNodesByName.Values)
            {
                if (node.ElementId.Contains('.') == false)
                    this.RenderFocusGraph(node.ElementId, cssFile, node, $"focus/{node.NodeName.FirstSlashPart()}");
            }
        }

        void RenderFocusGraph(String name,
            String cssFile,
            GraphNodeWrapper focusGraphNode,
            String traversalName)
        {
            SvgEditor e = new SvgEditor($"FocusGraph_{name}");
            e.AddCssFile(cssFile);

            this.svgEditors.Add(e);
            SENodeGroup seGroupParents = new SENodeGroup("parents", true);
            SENodeGroup seGroupFocus = new SENodeGroup("focus", true);
            SENodeGroup seGroupChildren = new SENodeGroup("children", true);
            seGroupParents.AppendChild(seGroupFocus);
            seGroupFocus.AppendChild(seGroupChildren);

            SENode focusSENode = this.CreateNode(focusGraphNode);
            focusSENode.Class = "focus";
            seGroupFocus.AppendNode(focusSENode);

            seGroupParents.AppendNodes(TraverseParents(focusGraphNode, focusSENode, "focus/*", 1));
            seGroupFocus.AppendChildren(TraverseChildren(focusGraphNode, focusSENode, "focus/*", 1));

            e.Render(seGroupParents, true);
        }

        protected SENode CreateNodeBinding(ElementDefinition.ElementDefinitionBindingComponent binding)
        {
            String hRef = null;
            //$if (linkFlag)
            //$    hRef = this.HRef(mapNode);
            SENode node = new SENode
            {
                HRef = hRef
            };
            node.Class = "valueSet";

            String displayName = binding.ValueSet.LastPathPart();
            if (this.valueSets.TryGetValue(binding.ValueSet, out ValueSet vs) == false)
            {
                displayName = vs.Name;
            }
            node.AddTextLine(displayName, hRef);
            node.AddTextLine("ValueSet", hRef);
            node.LhsAnnotation = "bind";
            return node;
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

            node.LhsAnnotation = ResolveAnnotation(graphNode, graphNode.LhsAnnotationText);
            node.RhsAnnotation = ResolveAnnotation(graphNode, graphNode.RhsAnnotationText);
            
            return node;
        }

        String ResolveCardinality(GraphNodeWrapper node,
            String elementId)
        {
            ElementDefinition e = FindElementDefinition(elementId);
            if (e == null)
                return null;

            if (e.Min.HasValue == false)
            {
                this.ConversionError("FGrapher",
                    "ResolveCardinality",
                    $"element {elementId}' min cardinality is empty");
                return null;
            }
            if (String.IsNullOrWhiteSpace(e.Max) == true)
            {
                this.ConversionError("FGrapher",
                    "ResolveCardinality",
                    $"element {elementId}' max cardinality is empty");
                return null;
            }
            return $"{e.Min.Value}..{e.Max}";
        }


        String ResolveAnnotation(GraphNodeWrapper node,
            String annotationSource)
        {
            if (String.IsNullOrEmpty(annotationSource))
                return null;
            switch (annotationSource[0])
            {
                case '^':
                    return ResolveCardinality(node, annotationSource.Substring(1));

                default:
                    return annotationSource;
            }
        }

        IEnumerable<SENode> TraverseParents(GraphNodeWrapper focusNode,
            SENode seFocusNode,
            String traversalFilter,
            Int32 depth)
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
                    SENode parent = CreateNode(parentLink.Node);
                    yield return parent;
                }
            }
        }

        IEnumerable<SENodeGroup> TraverseChildren(GraphNodeWrapper focusNode,
            SENode seFocusNode,
            String traversalFilter,
            Int32 depth)
        {
            Regex r = new Regex(traversalFilter);

            HashSet<GraphNodeWrapper> childNodes = new HashSet<GraphNodeWrapper>();
            childNodes.Add(focusNode);
            focusNode.ChildLinks.SortByTraversalName();

            foreach (GraphNodeWrapper.Link childLink in focusNode.ChildLinks)
            {
                if (
                    (childLink.Depth <= depth) &&
                    (r.IsMatch(childLink.Traversal.TraversalName)) &&
                    (childNodes.Contains(childLink.Node) == false)
                )
                {
                    SENodeGroup childContainer = new SENodeGroup("Child", true);
                    SENode child = CreateNode(childLink.Node);
                    childContainer.AppendNode(child);

                    childContainer.AppendChildren(TraverseChildren(childLink.Node,
                        child, 
                        traversalFilter,
                        depth - childLink.Depth));
                    yield return childContainer;
                }
            }
        }
    }
}
