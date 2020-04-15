using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace FSHer.Bbl.FSH
{
    public interface IDescription
    {
        String Description { get; set; }
    }

    public static class DescriptionExtensions
    {
        public static StringBuilder WriteDescription(this StringBuilder sb, IDescription item)
        {
            String description = item.Description.Replace("\r", "");
            String[] lines = description.Split('\n');
            switch (lines.Length)
            {
                case 0:
                    break;
                case 1:
                    sb.WriteLine(1, $"Description: \"{lines[0]}\"");
                    break;

                default:
                    sb.WriteLine(1, $"Description: \"\"\"");
                    foreach (String line in lines)
                        sb.WriteLine(2, $"\"{line}\"");
                    sb.WriteLine(2, $"\"\"\"");
                    break;
            }
            return sb;
        }
    }

}
