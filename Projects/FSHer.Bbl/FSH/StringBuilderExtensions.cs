using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

        public static StringBuilder WriteLine(this StringBuilder sb, 
            Int32 margin, 
            String s)
        {
            sb.WriteMargin(margin);
            return sb.WriteLine(s);
        }

        public static StringBuilder WriteLine(this StringBuilder sb, String s)
        {
            sb.AppendLine(s);
            return sb;
        }

        public static StringBuilder Write(this StringBuilder sb, String text)
        {
            sb.Append(text);
            return sb;
        }

        public static StringBuilder WriteMargin(this StringBuilder sb, Int32 margin)
        {
            for (Int32 i = 0; i < margin; i++)
                sb.Append("  ");
            return sb;
        }

        public static StringBuilder WritePaths(this StringBuilder sb,
            IEnumerable<String> strings)
        {
            if (strings == null)
                return sb;

            List<String> strList = strings.ToList();
            if (strList.Count == 0)
                return sb;

            sb.Append(strList[0]);
            for (Int32 i = 1; i < strList.Count; i++)
                sb.Append($", strList[i]");
            return sb;
        }

    }
}
