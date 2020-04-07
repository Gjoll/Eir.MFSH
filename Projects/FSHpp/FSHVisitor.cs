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
        Stack<NodeBase> current = new Stack<NodeBase>();
        VisitorInfo info;


        T GetCurrent<T>()
            where T : class
        {
            T retVal = this.current.Peek() as T;
            if (retVal == null)
                throw new Exception($"Internal Error. Can not convert current node {this.current.Peek().GetType().FullName} to {typeof(T).FullName}");
            return retVal;
        }

        public FSHVisitor(String fshText)
        {
            this.info = new VisitorInfo(fshText);
        }

        void PushAndVisit(NodeBase c,
            ParserRuleContext context)
        {
            this.info.PushSubString(0, context.Stop.StopIndex + 1);
            this.current.Push(c);
            this.VisitChildren(context);
            this.current.Pop();
            this.info.PopSubString();
        }

        void StoreCurrent(NodeBase b)
        {
            this.GetCurrent<IContainer>().Nodes.Add(b);
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
            n.Name = context.SEQUENCE().GetText();
            StoreCurrent(n);
            this.PushAndVisit(n, context);
            return null;
        }

        public override object VisitExtension(FSHParser.ExtensionContext context)
        {
            NodeExtension n = this.info.GetCode<NodeExtension>("alias", context);
            StoreCurrent(n);
            n.Name = context.SEQUENCE().GetText();
            this.PushAndVisit(n, context);
            return null;
        }

        public override object VisitInvariant(FSHParser.InvariantContext context)
        {
            NodeInvariant n = this.info.GetCode<NodeInvariant>("alias", context);
            n.Name = context.SEQUENCE().GetText();
            StoreCurrent(n);
            return null;
        }

        public override object VisitInstance(FSHParser.InstanceContext context)
        {
            NodeInstance n = this.info.GetCode<NodeInstance>("alias", context);
            n.Name = context.SEQUENCE().GetText();
            StoreCurrent(n);
            return null;
        }

        public override object VisitValueSet(FSHParser.ValueSetContext context)
        {
            NodeValueSet n = this.info.GetCode<NodeValueSet>("alias", context);
            n.Name = context.SEQUENCE().GetText();
            StoreCurrent(n);
            return null;
        }

        public override object VisitCodeSystem(FSHParser.CodeSystemContext context)
        {
            NodeCodeSystem n = this.info.GetCode<NodeCodeSystem>("alias", context);
            n.Name = context.SEQUENCE().GetText();
            StoreCurrent(n);
            return null;
        }

        public override object VisitRuleSet(FSHParser.RuleSetContext context)
        {
            NodeRuleSet n = this.info.GetCode<NodeRuleSet>("alias", context);
            n.Name = context.SEQUENCE().GetText();
            StoreCurrent(n);
            return null;
        }

        public override object VisitMapping(FSHParser.MappingContext context)
        {
            NodeMapping n = this.info.GetCode<NodeMapping>("alias", context);
            n.Name = context.SEQUENCE().GetText();
            StoreCurrent(n);
            return null;
        }


        #region MetaData
        public override object VisitParent(FSHParser.ParentContext context)
        {
            NodeParent n = this.info.GetCode<NodeParent>("parent", context);
            n.Name = context.SEQUENCE().GetText();
            this.GetCurrent<IMetadata>().Metadata.Add(n);
            return null;
        }

        public override object VisitId(FSHParser.IdContext context)
        {
            NodeId n = this.info.GetCode<NodeId>("id", context);
            n.Name = context.SEQUENCE().GetText();
            this.GetCurrent<IMetadata>().Metadata.Add(n);
            return null;
        }

        public override object VisitTitle(FSHParser.TitleContext context)
        {
            NodeTitle n = this.info.GetCode<NodeTitle>("title", context);
            n.Name = context.STRING().GetText();
            this.GetCurrent<IMetadata>().Metadata.Add(n);
            return null;
        }

        public override object VisitDescription(FSHParser.DescriptionContext context)
        {
            NodeDescription n = this.info.GetCode<NodeDescription>("description", context);
            n.Description = context.GetText();
            this.GetCurrent<IMetadata>().Metadata.Add(n);
            return null;
        }

        public override object VisitMixin(FSHParser.MixinContext context)
        {
            NodeMixin n = this.info.GetCode<NodeMixin>("mixin", context);
            n.Name = context.SEQUENCE().GetText();
            this.GetCurrent<IMetadata>().Metadata.Add(n);
            return null;
        }

        public override object VisitExpression(FSHParser.ExpressionContext context)
        {
            NodeExpression n = this.info.GetCode<NodeExpression>("expression", context);
            n.Expression = context.STRING().GetText();
            this.GetCurrent<IMetadata>().Metadata.Add(n);
            return null;
        }

        public override object VisitXpath(FSHParser.XpathContext context)
        {
            NodeXPath n = this.info.GetCode<NodeXPath>("xpath", context);
            n.XPath = context.STRING().GetText();
            this.GetCurrent<IMetadata>().Metadata.Add(n);
            return null;
        }

        public override object VisitSeverity(FSHParser.SeverityContext context)
        {
            NodeSeverity n = this.info.GetCode<NodeSeverity>("severity", context);
            n.Severity = context.CODE().GetText();
            this.GetCurrent<IMetadata>().Metadata.Add(n);
            return null;
        }

        public override object VisitInstanceOf(FSHParser.InstanceOfContext context)
        {
            NodeInstanceOf n = this.info.GetCode<NodeInstanceOf>("instanceOf", context);
            n.InstanceOf = context.SEQUENCE().GetText();
            this.GetCurrent<IMetadata>().Metadata.Add(n);
            return null;
        }

        public override object VisitUsage(FSHParser.UsageContext context)
        {
            NodeUsage n = this.info.GetCode<NodeUsage>("usage", context);
            n.Usage = context.CODE().GetText();
            this.GetCurrent<IMetadata>().Metadata.Add(n);
            return null;
        }

        public override object VisitSource(FSHParser.SourceContext context)
        {
            NodeSource n = this.info.GetCode<NodeSource>("source", context);
            n.Source = context.SEQUENCE().GetText();
            this.GetCurrent<IMetadata>().Metadata.Add(n);
            return null;
        }


        public override object VisitTarget(FSHParser.TargetContext context)
        {
            NodeTarget n = this.info.GetCode<NodeTarget>("target", context);
            n.Target = context.STRING().GetText();
            this.GetCurrent<IMetadata>().Metadata.Add(n);
            return null;
        }
        #endregion

        #region Rules
        public override object VisitCardRule(FSHParser.CardRuleContext context)
        {
            NodeCardRule n = this.info.GetCode<NodeCardRule>("cardRule", context);
            n.Contents = context.GetText();
            this.GetCurrent<IRule>().Rules.Add(n);
            return null;
        }

        public override object VisitFlagRule(FSHParser.FlagRuleContext context)
        {
            NodeFlagRule n = this.info.GetCode<NodeFlagRule>("flagRule", context);
            n.Contents = context.GetText();
            this.GetCurrent<IRule>().Rules.Add(n);
            return null;
        }

        public override object VisitValueSetRule(FSHParser.ValueSetRuleContext context)
        {
            NodeValueSetRule n = this.info.GetCode<NodeValueSetRule>("valueSetRule", context);
            n.Contents = context.GetText();
            this.GetCurrent<IRule>().Rules.Add(n);
            return null;
        }

        public override object VisitFixedValueRule(FSHParser.FixedValueRuleContext context)
        {
            NodeFixedValueRule n = this.info.GetCode<NodeFixedValueRule>("fixedValueRule", context);
            n.Contents = context.GetText();
            this.GetCurrent<IRule>().Rules.Add(n);
            return null;
        }

        public override object VisitContainsRule(FSHParser.ContainsRuleContext context)
        {
            NodeContainsRule n = this.info.GetCode<NodeContainsRule>("containsRule", context);
            n.Contents = context.GetText();
            this.GetCurrent<IRule>().Rules.Add(n);
            return null;
        }

        public override object VisitOnlyRule(FSHParser.OnlyRuleContext context)
        {
            NodeOnlyRule n = this.info.GetCode<NodeOnlyRule>("onlyRule", context);
            n.Contents = context.GetText();
            this.GetCurrent<IRule>().Rules.Add(n);
            return null;
        }

        public override object VisitObeysRule(FSHParser.ObeysRuleContext context)
        {
            NodeObeysRule n = this.info.GetCode<NodeObeysRule>("obeysRule", context);
            n.Contents = context.GetText();
            this.GetCurrent<IRule>().Rules.Add(n);
            return null;
        }

        public override object VisitCaretValueRule(FSHParser.CaretValueRuleContext context)
        {
            NodeCaretValueRule n = this.info.GetCode<NodeCaretValueRule>("caretValueRule", context);
            n.Contents = context.GetText();
            this.GetCurrent<IRule>().Rules.Add(n);
            return null;
        }

        public override object VisitMappingRule(FSHParser.MappingRuleContext context)
        {
            NodeMappingRule n = this.info.GetCode<NodeMappingRule>("mappingRule", context);
            n.Contents = context.GetText();
            this.GetCurrent<IRule>().Rules.Add(n);
            return null;
        }

        public override object VisitRuleSetReference(FSHParser.RuleSetReferenceContext context)
        {
            NodeMappingRule n = this.info.GetCode<NodeMappingRule>("ruleSetReference", context);
            n.Contents = context.GetText();
            this.GetCurrent<IRule>().Rules.Add(n);
            return null;
        }

        #endregion

    }
}
