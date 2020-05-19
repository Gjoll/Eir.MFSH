using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Eir.MFSH
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
                    String[] parts = token.Split('=');
                    Int32 tNum = Int32.Parse(parts[1]);
                    if (!this.tokenDict.ContainsKey(tNum))
                        this.tokenDict.Add(tNum, parts[0]);
                }
            }

            return this.tokenDict[tokenNum];
        }
    }
}
