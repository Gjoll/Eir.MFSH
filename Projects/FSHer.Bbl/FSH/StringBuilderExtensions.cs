using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSHer.Bbl.FSH
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder WriteOptionalItemList(this StringBuilder sb,
            Int32 margin,
            String name,
            IEnumerable<String> values)
        {
            if (values == null)
                return sb;
            if (!values.Any())
                return sb;

            String[] valueArray = values.ToArray();

            String comma = valueArray.Length == 1 ? "" : ",";
            WriteLine(sb, 1, $"{name}: {valueArray[0]}{comma}");
            for (Int32 i = 1; i < valueArray.Length - 1; i++)
                WriteLine(sb, 2, $"{valueArray[i]},");
            WriteLine(sb, 2, $"{valueArray.Last()}");

            return sb;
        }

        public static StringBuilder WriteOptionalItem(this StringBuilder sb,
            Int32 margin, 
            String name, 
            String value)
        {
            if (String.IsNullOrEmpty(value))
                return sb;
            return sb.WriteLine(margin, $"{name}: {value}");
        }

        public static StringBuilder WriteLine(this StringBuilder sb, Int32 margin, String s)
        {
            for (Int32 i = 0; i < margin; i++)
                sb.Append("  ");
            sb.AppendLine(s);
            return sb;
        }
    }
}
