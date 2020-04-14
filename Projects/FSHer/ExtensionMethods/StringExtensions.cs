using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eir.FSHer
{
    public static class StringExtensions
    {
        public static IEnumerable<String> RemoveQuotes(this IEnumerable<String> strings)
        {
            foreach (String s in strings)
            {
                String t = s.Trim();
                if (t.StartsWith("\""))
                    t = t.Substring(1);
                if (t.EndsWith("\""))
                    t = t.Substring(0, t.Length - 1);
                yield return t;
            }
        }
    }
}
