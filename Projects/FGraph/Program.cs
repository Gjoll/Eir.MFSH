using System;
using System.IO;

namespace FGraph
{
    class Program
    {
        FGrapher fGrapher;

        public Program()
        {
            this.fGrapher = new FGrapher();
            this.fGrapher.OutputDir = Path.GetFullPath(".\\Graphs");
            this.fGrapher.ConsoleLogging();
        }

        void ParseArguments(String[] args)
        {
            Int32 i = 0;

            String GetArg(String errorMsg)
            {
                if (i >= args.Length)
                    throw new Exception($"Missing {errorMsg} parameter");

                String arg = args[i++].Trim();
                if (arg.Length > 0)
                {
                    if ((arg[0] == '"') && (arg[arg.Length - 1] == '"'))
                        arg = arg.Substring(1, arg.Length - 2);
                }

                return arg;
            }

            while (i < args.Length)
            {
                String arg = GetArg("arg").ToUpper();
                switch (arg.ToLower())
                {
                    case "-g":
                        String graphName= GetArg("-g");
                        this.fGrapher.GraphName = graphName;
                        break;

                    case "-i":
                        String pathArg = GetArg("-i");
                        this.fGrapher.Load(pathArg);
                        break;

                    case "-o":
                        this.fGrapher.OutputDir = GetArg("-o");
                        break;

                    default:
                        throw new Exception($"Unknown arg {arg}");
                }
            }
        }

        bool Process()
        {
            this.fGrapher.Process();
            //this.fGrapher.SaveAll();
            return this.fGrapher.HasErrors == false;
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
