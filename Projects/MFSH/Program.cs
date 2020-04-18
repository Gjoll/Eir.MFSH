using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFSH
{
    class Program
    {
        MFsh pp = new MFsh();
        String outputDir = ".";

        public Program()
        {
            this.pp.ConsoleLogging();
        }

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

            String filter = ".mfsh";
            while (i < args.Length)
            {
                String arg = GetArg("arg").ToUpper();
                switch (arg.ToLower())
                {
                    case "-f":
                        filter = GetArg("-f");
                        break;

                    case "-d":
                        this.pp.AddDir(GetArg("-d"), filter);
                        break;

                    case "-o":
                        this.outputDir = GetArg("-0");
                        break;

                    default:
                        throw new Exception($"Unknown arg {arg}");
                }
            }
        }

        bool Process()
        {
            //if (pp.Process() == false)
            //    return false;

            //this.pp.SaveAll(this.outputDir);
            return true;
        }

        static Int32 Main(string[] args)
        {
            try
            {
                Program p = new Program();
                p.ParseArguments(args);
                if (p.Process() == false)
                    return -1;
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
