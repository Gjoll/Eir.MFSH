using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;

namespace Eir.MFSH.Parser
{
    public class TokenLookup
    {
        private String tokenFile;
        Dictionary<Int32, String> tokenDict = null;

        public TokenLookup(String tokenFile)
        {
            this.tokenFile = tokenFile;
        }

        public String Lookup(Int32 tokenNum)
        {
            if (this.tokenDict == null)
            {
                this.tokenDict = new Dictionary<int, string>();
                this.tokenDict.Add(-1, "<eof>");

                String tokenPath = Path.Combine(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    this.tokenFile);
                foreach (String token in File.ReadAllLines(tokenPath))
                {
                    // If token name contains '=', then name is put in single quotes, otherwise not.
                    String tName;
                    String tNumStr;
                    String t = token.Trim();
                    if (t[0] == '\'')
                    {
                        Int32 index = t.IndexOf('\'', 2);
                        tName = t.Substring(1, index - 1);
                        tNumStr = t.Substring(t.IndexOf('=', index) + 1);
                    }
                    else
                    {
                        String[] parts = t.Split('=');
                        tName = parts[0];
                        tNumStr = parts[1];
                    }

                    Int32 tNum = Int32.Parse(tNumStr);
                    if (!this.tokenDict.ContainsKey(tNum))
                        this.tokenDict.Add(tNum, tName);
                }
            }

            return this.tokenDict[tokenNum];
        }
    }
}
