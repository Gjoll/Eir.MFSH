﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MFSH.Antlr
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
                foreach (String token in File.ReadAllLines(this.tokenFile))
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