using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eir.FSHer
{
    public static class NodeListExtensions
    {
        /// <summary>
        /// Return the children of the nodes.
        /// </summary>
        public static IEnumerable<NodeBase> Children(this IEnumerable<NodeRule> items)
        {
            foreach (NodeRule item in items.Rules())
            {
                foreach (NodeBase child in item.ChildNodes)
                    yield return child;
            }
        }

        /// <summary>
        /// Return only the NodeTokens in list with the NodeType.
        /// </summary>
        public static IEnumerable<NodeRule> WithRuleName(this IEnumerable<NodeRule> items,
            String nodeType)
        {
            foreach (NodeRule item in items)
            {
                if (item.RuleName == nodeType)
                    yield return item;
            }
        }

        /// <summary>
        /// Return only the NodeTokens in list with the TokenName.
        /// </summary>
        public static IEnumerable<NodeToken> WithTokenName(this IEnumerable<NodeToken> items,
            String tokenName)
        {
            foreach (NodeToken item in items)
            {
                if (item.TokenName == tokenName)
                    yield return item;
            }
        }

        /// <summary>
        /// Return only the NodeTokens in list.
        /// </summary>
        public static IEnumerable<NodeToken> Tokens(this IEnumerable<NodeBase> items)
        {
            foreach (NodeBase item in items)
            {
                NodeToken t = item as NodeToken;
                if (t != null)
                    yield return t;
            }
        }

        /// <summary>
        /// Return only the NodeRules in list.
        /// </summary>
        public static IEnumerable<NodeRule> Rules(this IEnumerable<NodeBase> items)
        {
            foreach (NodeBase item in items)
            {
                NodeRule r = item as NodeRule;
                if (r != null)
                    yield return r;
            }
        }

        /// <summary>
        /// Return all rule children that are of indicated type.
        /// </summary>
        public static IEnumerable<NodeRule> ExcludeRules(this IEnumerable<NodeRule> items,
            String ruleName)
        {
            foreach (NodeRule item in items)
            {
                if (item.RuleName != ruleName)
                    yield return item;
            }
        }

        /// <summary>
        /// Return all rule children that are of indicated type.
        /// </summary>
        public static IEnumerable<NodeRule> HasRuleChildOfType(this IEnumerable<NodeRule> items,
            String nodeType)
        {
            foreach (NodeRule item in items)
            {
                if (item.ChildNodes.Rules().WithRuleName(nodeType).Any())
                    yield return item;
            }
        }
    }
}
