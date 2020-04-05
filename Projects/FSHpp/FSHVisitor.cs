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
            NodeDocument doc = new NodeDocument();
            doc.Comments = this.info.GetComments(context);
            this.PushAndVisit(doc, context);
            this.info.End("doc", context);
            doc.TrailingText = this.info.CopyToEnd();
            return doc;
        }

        public override object VisitAlias(FSHParser.AliasContext context)
        {
            NodeAlias n = this.info.GetCode<NodeAlias>("alias", context);
            StoreCurrent(n);
            n.Name = context.SEQUENCE(0).GetText();
            n.Value= context.SEQUENCE(1).GetText();
            return null;
        }

        public override object VisitProfile(FSHParser.ProfileContext context)
        {
            NodeProfile n = this.info.GetCode<NodeProfile>("alias", context);
            StoreCurrent(n);
            return null;
        }

        public override object VisitExtension(FSHParser.ExtensionContext context)
        {
            NodeExtension n = this.info.GetCode<NodeExtension>("alias", context);
            StoreCurrent(n);
            n.Name = context.SEQUENCE().GetText();
            return null;
        }

        public override object VisitInvariant(FSHParser.InvariantContext context)
        {
            NodeInvariant n = this.info.GetCode<NodeInvariant>("alias", context);
            StoreCurrent(n);
            n.Name = context.SEQUENCE().GetText();
            return null;
        }

        public override object VisitInstance(FSHParser.InstanceContext context)
        {
            NodeInstance n = this.info.GetCode<NodeInstance>("alias", context);
            StoreCurrent(n);
            n.Name = context.SEQUENCE().GetText();
            return null;
        }

        public override object VisitValueSet(FSHParser.ValueSetContext context)
        {
            NodeValueSet n = this.info.GetCode<NodeValueSet>("alias", context);
            StoreCurrent(n);
            n.Name = context.SEQUENCE().GetText();
            return null;
        }

        public override object VisitCodeSystem(FSHParser.CodeSystemContext context)
        {
            NodeCodeSystem n = this.info.GetCode<NodeCodeSystem>("alias", context);
            StoreCurrent(n);
            n.Name = context.SEQUENCE().GetText();
            return null;
        }

        public override object VisitRuleSet(FSHParser.RuleSetContext context)
        {
            NodeRuleSet n = this.info.GetCode<NodeRuleSet>("alias", context);
            StoreCurrent(n);
            n.Name = context.SEQUENCE().GetText();
            return null;
        }

        public override object VisitMapping(FSHParser.MappingContext context)
        {
            NodeMapping n = this.info.GetCode<NodeMapping>("alias", context);
            StoreCurrent(n);
            n.Name = context.SEQUENCE().GetText();
            return null;
        }
    }
}
