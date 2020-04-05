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
            NodeAlias n = this.info.Code<NodeAlias>("alias", context);
            StoreCurrent(n);
            n.Name = context.SEQUENCE(0).GetText();
            n.Value= context.SEQUENCE(1).GetText();
            return null;
        }

        public override object VisitProfile(FSHParser.ProfileContext context)
        {
            NodeCode n = this.info.Code<NodeCode>("alias", context);
            StoreCurrent(n);
            return null;
        }

        public override object VisitExtension(FSHParser.ExtensionContext context)
        {
            NodeExtension n = this.info.Code<NodeExtension>("alias", context);
            StoreCurrent(n);
            n.Name = context.SEQUENCE().GetText();
            return null;
        }

        public override object VisitInvariant(FSHParser.InvariantContext context)
        {
            NodeInvariant n = this.info.Code<NodeInvariant>("alias", context);
            StoreCurrent(n);
            n.Name = context.SEQUENCE().GetText();
            return null;
        }

        public override object VisitInstance(FSHParser.InstanceContext context)
        {
            NodeInstance n = this.info.Code<NodeInstance>("alias", context);
            StoreCurrent(n);
            n.Name = context.SEQUENCE().GetText();
            return null;
        }

        public override object VisitValueSet(FSHParser.ValueSetContext context)
        {
            NodeValueSet n = this.info.Code<NodeValueSet>("alias", context);
            StoreCurrent(n);
            n.Name = context.SEQUENCE().GetText();
            return null;
        }

        public override object VisitCodeSystem(FSHParser.CodeSystemContext context)
        {
            NodeCodeSystem n = this.info.Code<NodeCodeSystem>("alias", context);
            StoreCurrent(n);
            n.Name = context.SEQUENCE().GetText();
            return null;
        }

        public override object VisitRuleSet(FSHParser.RuleSetContext context)
        {
            NodeRuleSet n = this.info.Code<NodeRuleSet>("alias", context);
            StoreCurrent(n);
            n.Name = context.SEQUENCE().GetText();
            return null;
        }

        public override object VisitMapping(FSHParser.MappingContext context)
        {
            NodeMapping n = this.info.Code<NodeMapping>("alias", context);
            StoreCurrent(n);
            n.Name = context.SEQUENCE().GetText();
            return null;
        }
    }
}
