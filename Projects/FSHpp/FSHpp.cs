using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp
{
    public class FSHpp
    {
        public void ProcessFile(String path)
        {
        }

        public void ProcessDir(String path, String filter)
        {
            foreach (String subDir in Directory.GetDirectories(path))
                ProcessDir(subDir, filter);

            foreach (String file in Directory.GetFiles(path, filter))
                ProcessFile(file);
        }
    }

}
