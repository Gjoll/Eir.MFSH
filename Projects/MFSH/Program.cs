using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;

namespace MFSH
{
    class Program
    {
        MFsh mfsh;

        public Program()
        {
            this.mfsh = new MFsh();
            this.mfsh.BaseInputDir = Path.GetFullPath(".");
            this.mfsh.BaseOutputDir = Path.GetFullPath(".");
            this.mfsh.ConsoleLogging();
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
                String DirPart(String path)
                {
                    if (Directory.Exists(path))
                        return path;
                    else
                        return Path.GetDirectoryName(path);
                }

                String arg = GetArg("arg").ToUpper();
                switch (arg.ToLower())
                {
                    case "-i":
                        this.mfsh.IncludeDirs.Add(GetArg("-i"));
                        break;

                    case "-d":
                        {
                            String name = GetArg("-d");
                            String value = GetArg("-d");
                            name = name.Replace("%%", "%");
                            value = value.Replace("%%", "%");
                            this.mfsh.GlobalVars.Set(name, value);
                        }
                        break;

                    case "-b":
                        this.mfsh.BaseInputDir = GetArg("-b");
                        break;

                    case "-p":
                        {
                            String pathArg = GetArg("-p");
                            this.mfsh.Paths.Add(pathArg);
                            if (String.IsNullOrEmpty(this.mfsh.BaseInputDir) == true)
                                this.mfsh.BaseInputDir = DirPart(pathArg);
                        }
                        break;

                    case "-o":
                        this.mfsh.BaseOutputDir = GetArg("-o");
                        break;

                    default:
                        throw new Exception($"Unknown arg {arg}");
                }
            }
        }

        bool Process()
        {
            this.mfsh.Process();
            this.mfsh.SaveAll();
            return this.mfsh.HasErrors == false;
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
