using System;
using System.Collections.Generic;
using System.Text;

namespace FGraph
{
    public class GraphTraversal
    {
        public String LinkGraphName { get; set; }
        public String LinkSource { get; set; }
        public String LinkTarget { get; set; }

        public GraphTraversal()
        {
        }

        public GraphTraversal(dynamic data)
        {
            this.LinkGraphName = data.linkGraphName;
            this.LinkSource = data.linkSource;
            this.LinkTarget = data.linkTarget;
        }
    }
}
