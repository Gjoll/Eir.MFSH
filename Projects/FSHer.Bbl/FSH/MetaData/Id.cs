using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace FSHer.Bbl.FSH
{
    public interface IId
    {
        String Id { get; set; }
    }

    public static class IdExtensions
    {
        public static StringBuilder WriteId(this StringBuilder sb, IId item)
        {
            sb.WriteOptionalItem(1, "Id", item.Id);
            return sb;
        }
    }

}
