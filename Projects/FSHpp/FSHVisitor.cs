using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp
{
    class FSHVisitor : FSHBaseVisitor<object>
    {
        private VisitorInfo info;

        public FSHVisitor(VisitorInfo info)
        {
            this.info = info;
        }

        public override object VisitDoc([NotNull] FSHParser.DocContext context)
        {
            return null;
        }

    }
}
