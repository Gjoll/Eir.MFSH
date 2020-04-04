using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSHpp
{
    class Program
    {
        FSHpp pp  = new FSHpp();

        void ParseArguments(String[] args)
        {
            Int32 i = 0;

            String GetArg(String errorMsg)
            {
                if (i >= args.Length)
                    throw new Exception($"Missing {errorMsg} parameter");

                String arg = args[i++].Trim();
                if ((arg[0] == '"') && (arg[arg.Length - 1] == '"'))
                    arg = arg.Substring(1, arg.Length - 2);
                return arg;
            }

            String filter = ".fshpp";
            while (i < args.Length)
            {
                String arg = GetArg("arg").ToUpper();
                switch (arg)
                {
                    case "-f":
                        filter = GetArg("-f");
                        break;

                    case "-d":
                        this.pp.ProcessDir(GetArg("-d"), filter);
                        break;

                    default:
                        throw new Exception($"Unknown arg {arg}");
                }
            }
        }

        static Int32 Main(string[] args)
        {
            try
            {
                Program p = new Program();
                p.ParseArguments(args);
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
