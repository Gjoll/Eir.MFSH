using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace FSHpp
{
    class FSHVisitor : FSHBaseVisitor<object>
    {
        Stack<NodeContainer> current = new Stack<NodeContainer>();

        private VisitorInfo info;


        public FSHVisitor(VisitorInfo info)
        {
            this.info = info;
        }

        void PushAndVisit(NodeContainer c,
            IRuleNode context)
        {
            this.current.Push(c);
            this.VisitChildren(context);
            this.current.Pop();
        }

        void StoreCurrent(NodeBase b)
        {
            this.current.Peek().Nodes.Add(b);
        }

        public override object VisitDoc([NotNull] FSHParser.DocContext context)
        {
            NodeDocument doc = this.info.Start<NodeDocument>("doc", context);
            this.PushAndVisit(doc, context);
            this.info.End("doc", context);
            return null;
        }

        public override object VisitAlias(FSHParser.AliasContext context)
        {
            StoreCurrent(this.info.Code("alias", context));
            return null;
        }
    }
}
