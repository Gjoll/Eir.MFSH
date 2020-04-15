using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace FSHer.Bbl.FSH
{
    public interface IParent
    {
        String Parent { get; set; }
    }

    public static class ParentExtensions
    {
        public static StringBuilder WriteParent(this StringBuilder sb, IParent item)
        {
            sb.WriteOptionalItem(1, "Parent", item.Parent);
            return sb;
        }
    }

}
