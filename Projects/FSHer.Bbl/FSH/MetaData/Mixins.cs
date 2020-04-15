using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace FSHer.Bbl.FSH
{
    public interface IMixins
    {
        IEnumerable<String> Mixins { get; set; }
    }

    public static class MixinsExtensions
    {
        public static StringBuilder WriteMixins(this StringBuilder sb, IMixins item)
        {
            sb.WriteOptionalItemList(1, "Mixins", item.Mixins);
            return sb;
        }
    }

}
