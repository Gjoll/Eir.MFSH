using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using FSHpp.Nodes;

namespace FSHpp
{
    class FSHVisitor : FSHBaseVisitor<object>
    {
        private VisitorInfo info;

        public FSHVisitor(VisitorInfo info)
        {
            this.info = info;
        }

        NodeCode Code(ParserRuleContext context)
        {
            NodeCode retVal = new NodeCode();
            Int32 length = context.Start.StartIndex - this.info.InputIndex;
            retVal.Comments = this.info.Input.Substring(this.info.InputIndex, length);

            this.info.InputIndex = context.Start.StartIndex;
            length = context.Stop.StopIndex - this.info.InputIndex;
            retVal.Code = this.info.Input.Substring(this.info.InputIndex, length);
            this.info.InputIndex = context.Stop.StopIndex + 1;
            return retVal;
        }

        T Start<T>(ParserRuleContext context)
            where T : NodeBase, new() 
        {
            T retVal = new T();
            Int32 length = context.Start.StartIndex - this.info.InputIndex;
            retVal.Comments = this.info.Input.Substring(this.info.InputIndex, length);
            return retVal;
        }

        void End(ParserRuleContext context)
        {
            this.info.InputIndex = context.Stop.StopIndex + 1;
        }
        public override object VisitDoc([NotNull] FSHParser.DocContext context)
        {
            NodeDocument doc = this.Start<NodeDocument>(context);
            this.VisitChildren(context);
            this.End(context);
            return doc;
        }

        public override object VisitEntity(FSHParser.EntityContext context)
        {
            NodeCode retVal = this.Code(context);
            this.VisitChildren(context);
            return retVal;
        }

        public override object VisitAlias(FSHParser.AliasContext context)
        {
            Debugger.Break();
            NodeCode retVal = this.Code(context);
            return retVal;
        }
    }
}
