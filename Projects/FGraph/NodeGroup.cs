using System;
using System.Collections.Generic;
using System.Text;

namespace FGraph
{
    public class NodeGroup
    {
        public List<NodeGroup> Parents { get; }  = new List<NodeGroup>();
        public List<NodeGroup> Children { get; } = new List<NodeGroup>();
        public List<GraphNode> Items { get; } = new List<GraphNode>();
    }
}
