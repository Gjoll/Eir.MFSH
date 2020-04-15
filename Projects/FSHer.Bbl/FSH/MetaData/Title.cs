using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace FSHer.Bbl.FSH
{
    public interface ITitle
    {
        String Title { get; set; }
    }

    public static class TitleExtensions
    {
        public static StringBuilder WriteTitle(this StringBuilder sb, ITitle item)
        {
            sb.WriteOptionalItem(1, "Title", item.Title);
            return sb;
        }
    }

}
