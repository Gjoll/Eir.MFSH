using System;
using System.Collections.Generic;
using System.Text;

namespace FGraph
{
    /// <summary>
    /// Contains all the nodes that are in one group inside of a graph column.
    /// </summary>
    public class GraphItemGroup
    {
        public List<GraphItemGroup> Parents { get; }  = new List<GraphItemGroup>();
        public List<GraphItemGroup> Children { get; } = new List<GraphItemGroup>();
        public List<GraphNode> Nodes { get; } = new List<GraphNode>();
    }
}
