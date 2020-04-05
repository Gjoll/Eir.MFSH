using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace FSHpp
{
    public static class ParserExtensionscs
    {
        public static IEnumerable<T> VisitMultipleChildren<T>(this AbstractParseTreeVisitor<Object> tree,
            ParserRuleContext[] contexts)
        {
            List<T> retVal = new List<T>();
            foreach (ParserRuleContext context in contexts)
            {
                retVal.Append((T) tree.VisitChildren(context));
            }
            return retVal;
        }
    }
}
