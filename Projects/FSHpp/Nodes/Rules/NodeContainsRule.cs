﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp
{
    class NodeContainsRule : NodeBase
    {
        public String Contents;
        public NodeContainsRule() { this.NodeType = "containsRule"; }
    }
}
