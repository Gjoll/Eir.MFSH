using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using FSHpp.Nodes;

namespace FSHpp
{
    class FSHListener : FSHBaseListener
    {
        public NodeDocument Doc;

        String text;
        Int32 textIndex;
        Stack<NodeBase> nodeStack = new Stack<NodeBase>();
        NodeBase current => this.nodeStack.Peek();

        public FSHListener(String text)
        {
            this.text = text;
            this.textIndex = 0;
            this.Doc = this.Push<NodeDocument>(0);
        }

        T Push<T>(Int32 startIndex)
            where T : NodeBase, new()
        {
            this.AppendComment(startIndex);
            T retVal = new T();
            this.nodeStack.Push(retVal);
            return retVal;
        }

        T Pop<T>()
            where T : NodeBase, new()
        {
            return (T)this.nodeStack.Pop();
        }

        /// <summary>
        /// Copy all bytes from current position up to but not
        /// including index position and return it.
        /// Update index
        /// </summary>
        String Consume(Int32 index)
        {
            Int32 length = index - this.textIndex;

            if (length < 0)
                throw new Exception("Internal index error");
            String retVal = this.text.Substring(this.textIndex, length);
            this.textIndex = index;
            //Trace.WriteLine($"  consumed {this.textIndex} .. {index}");
            return retVal;
        }

        void AppendNode(NodeBase node)
        {
            this.current.ChildNodes.Add(node);
            //Trace.WriteLine($"Node: {node.NodeType}");
        }

        /// <summary>
        /// Append a comment of all chars up to but not including index. 
        /// </summary>
        void AppendComment(Int32 index)
        {
            String comment = Consume(index);
            if (String.IsNullOrEmpty(comment))
                return;

            NodeComment node = new NodeComment
            {
                Comment = comment
            };
            this.current.ChildNodes.Add(node);
        }

        /// <summary>
        /// Appends a NodeToken to current, and sets the
        /// text of that token.
        /// </summary>
        NodeToken AppendToken(Int32 start, Int32 stop)
        {
            NodeToken retVal = AppendNode<NodeToken>(start);
            retVal.Token = this.Consume(stop);
            return retVal;
        }

        /// <summary>
        /// Append a comment of all chars up to but not including index.
        /// then create new node and append it.
        /// Does not read the value of the node in any way.
        /// The caller must set that up.
        /// </summary>
        T AppendNode<T>(Int32 index)
            where T : NodeBase, new()
        {
            this.AppendComment(index);
            T retVal = new T();
            this.current.ChildNodes.Add(retVal);
            return retVal;
        }

        public override void EnterDoc(FSHParser.DocContext context)
        {
            this.AppendComment(context.Start.StartIndex);
        }

        public override void ExitDoc(FSHParser.DocContext context)
        {
        }


        #region Entities
        // alias | profile | extension | invariant | instance | valueSet | codeSystem | ruleSet | mapping;

        void EnterEntityName(IToken token)
        {
            this.AppendComment(token.StartIndex);
            NodeEntityName node = this.AppendNode<NodeEntityName>(token.StartIndex);
            node.EntityName = Consume(token.StopIndex + 1);
        }

        public override void EnterAlias(FSHParser.AliasContext context)
        {
            this.Push<NodeAlias>(context.Start.StartIndex);
        }

        public override void ExitAlias(FSHParser.AliasContext context)
        {
            NodeAlias node = this.Pop<NodeAlias>();
        }

        public override void EnterAliasName(FSHParser.AliasNameContext context)
        {
            EnterEntityName(context.Start);
        }

        public override void EnterProfile(FSHParser.ProfileContext context)
        {
            this.Push<NodeProfile>(context.Start.StartIndex);
        }

        public override void ExitProfile(FSHParser.ProfileContext context)
        {
            this.Pop<NodeProfile>();
        }

        public override void EnterExtension(FSHParser.ExtensionContext context)
        {
            this.Push<NodeExtension>(context.Start.StartIndex);
        }

        public override void ExitExtension(FSHParser.ExtensionContext context)
        {
            this.Pop<NodeExtension>();
        }
        public override void EnterInvariant(FSHParser.InvariantContext context)
        {
            this.Push<NodeInvariant>(context.Start.StartIndex);
        }

        public override void ExitInvariant(FSHParser.InvariantContext context)
        {
            this.Pop<NodeInvariant>();
        }
        public override void EnterInstance(FSHParser.InstanceContext context)
        {
            this.Push<NodeInstance>(context.Start.StartIndex);
        }

        public override void ExitInstance(FSHParser.InstanceContext context)
        {
            this.Pop<NodeInstance>();
        }
        public override void EnterValueSet(FSHParser.ValueSetContext context)
        {
            this.Push<NodeValueSet>(context.Start.StartIndex);
        }

        public override void ExitValueSet(FSHParser.ValueSetContext context)
        {
            this.Pop<NodeValueSet>();
        }
        public override void EnterCodeSystem(FSHParser.CodeSystemContext context)
        {
            this.Push<NodeCodeSystem>(context.Start.StartIndex);
        }

        public override void ExitCodeSystem(FSHParser.CodeSystemContext context)
        {
            this.Pop<NodeCodeSystem>();
        }
        public override void EnterRuleSet(FSHParser.RuleSetContext context)
        {
            this.Push<NodeRuleSet>(context.Start.StartIndex);
        }

        public override void ExitRuleSet(FSHParser.RuleSetContext context)
        {
            this.Pop<NodeRuleSet>();
        }
        public override void EnterMapping(FSHParser.MappingContext context)
        {
            this.Push<NodeMapping>(context.Start.StartIndex);
        }

        public override void ExitMapping(FSHParser.MappingContext context)
        {
            this.Pop<NodeMapping>();
        }
        #endregion
        #region Tokens
        public override void EnterSequence(FSHParser.SequenceContext context)
        {
            NodeSequence node = this.AppendNode<NodeSequence>(context.Start.StartIndex);
            node.Text = this.Consume(context.Start.StopIndex + 1);
        }

        public override void EnterEqual(FSHParser.EqualContext context)
        {
            base.EnterEqual(context);
            this.AppendToken(context.Start.StartIndex, context.Start.StopIndex);
        }

        public override void EnterEveryRule(ParserRuleContext context)
        {
            Trace.WriteLine($"EnterEveryRule: {context.GetText()}");
            base.EnterEveryRule(context);
        }

        public override void ExitEveryRule(ParserRuleContext context)
        {
            base.ExitEveryRule(context);
            Trace.WriteLine($"ExitEveryRule: {context.GetText()}");
        }

        public override void VisitTerminal(ITerminalNode node)
        {
            Trace.WriteLine($"VisitTerminal: {node.GetText()}");
            base.VisitTerminal(node);
        }

        #endregion

    }
}
