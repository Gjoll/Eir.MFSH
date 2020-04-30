using System;
using System.Collections.Generic;
using System.Text;

namespace FGraph.Extensions
{
    public static class GraphNodeExtensions
    {
        public static void SortByTraversalName(this List<GraphNodeWrapper.Link> links)
        {
            links.Sort((a, b) => { return String.Compare(a.Traversal.TraversalName, b.Traversal.TraversalName); });
        }
    }
}
