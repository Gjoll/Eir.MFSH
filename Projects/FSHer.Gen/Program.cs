using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Eir.FSHerGen
{
    class Program
    {
       static Int32 Main(string[] args)
        {
            try
            {
                Generate g = new Generate();
                g.GenerateListener();
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
        }
    }
}
