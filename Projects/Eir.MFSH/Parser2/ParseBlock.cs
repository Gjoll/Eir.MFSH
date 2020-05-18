using System;
using System.Collections.Generic;
using System.Text;

namespace MFSH.Parser2
{
    public class ParseBlock
    {
        public String OutputPath { get; set; }
        public FileData Data;

        public ParseBlock()
        {
            this.Data = new FileData
            {
                RelativePathType = FileData.RedirType.Text
            };
        }
    }
}
