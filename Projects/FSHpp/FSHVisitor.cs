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
            doc.TrailingText = this.info.CopyToEnd();
            return doc;
        }

        public override object VisitAlias(FSHParser.AliasContext context)
        {
            StoreCurrent(this.info.Code("alias", context));
            return null;
        }

        public override object VisitProfile(FSHParser.ProfileContext context)
        {
            StoreCurrent(this.info.Code("profile", context));
            return null;
        }

        public override object VisitExtension(FSHParser.ExtensionContext context)
        {
            StoreCurrent(this.info.Code("extension", context));
            return null;
        }

        public override object VisitInvariant(FSHParser.InvariantContext context)
        {
            StoreCurrent(this.info.Code("invariant", context));
            return null;
        }

        public override object VisitInstance(FSHParser.InstanceContext context)
        {
            StoreCurrent(this.info.Code("instance", context));
            return null;
        }

        public override object VisitValueSet(FSHParser.ValueSetContext context)
        {
            StoreCurrent(this.info.Code("valueSet", context));
            return null;
        }

        public override object VisitCodeSystem(FSHParser.CodeSystemContext context)
        {
            StoreCurrent(this.info.Code("codeSystem", context));
            return null;
        }

        public override object VisitRuleSet(FSHParser.RuleSetContext context)
        {
            StoreCurrent(this.info.Code("ruleSet", context));
            return null;
        }

        public override object VisitMapping(FSHParser.MappingContext context)
        {
            StoreCurrent(this.info.Code("mapping", context));
            return null;
        }
    }
}
