using System;
using System.Collections.Generic;
using System.Text;

namespace FGraph
{
    static class StringExtensions
    {
        public static String FirstSlashPart(this string s)
        {
            String[] parts = s.Split('/');
            return parts[0];
        }
    }
}
