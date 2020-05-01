using System;
using System.Collections.Generic;
using System.IO;
using Hl7.Fhir.Model;

namespace FGraph
{
    class Program
    {
        FGrapher fGrapher;
        String inputPath = null;
        String inputDir= null;
        List<Tuple<String, String>> renderings = new List<Tuple<String, String>>();

        public Program()
        {
            this.fGrapher = new FGrapher();
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
                        if (this.fGrapher.GraphName != null)
                            throw new Exception("-o option can only be used once.");
                        this.fGrapher.GraphName = GetArg("-g");
                        break;

                    case "-i":
                        if (inputPath != null)
                            throw new Exception("-i option can only be used once.");
                        inputPath = GetArg("-i");
                        this.fGrapher.Load(inputPath);
                        if (Directory.Exists(inputPath))
                            this.inputDir = this.inputPath;
                        else
                            this.inputDir = Path.GetDirectoryName(this.inputPath);
                        break;

                    case "-o":
                        if (this.fGrapher.OutputDir != null)
                            throw new Exception("-o option can only be used once.");
                        this.fGrapher.OutputDir = GetArg("-o");
                        break;

                    case "-r":
                        this.renderings.Add(new Tuple<string, string>(GetArg("-t"), GetArg("-t")));
                        break;

                    default:
                        throw new Exception($"Unknown arg {arg}");
                }
            }
        }

        bool Process()
        {
            this.fGrapher.Process();
            foreach (Tuple<String, String> rendering in this.renderings)
            {
                bool Exists(String dir, ref String relativePath)
                {
                    String checkPath = Path.Combine(dir, relativePath);
                    if (File.Exists(checkPath) == false)
                        return false;
                    relativePath = checkPath;
                    return true;
                }

                String cssFile = rendering.Item2;
                if (
                    (!Exists(Path.GetFullPath("."), ref cssFile)) &&
                    (!Exists(this.inputPath, ref cssFile))
                )
                    throw new Exception($"Css file '{cssFile}' not found");

                switch (rendering.Item1.Trim().ToLower())
                {
                    case "focus":
                        this.fGrapher.RenderFocusGraphs(cssFile);
                        break;
                    default:
                        throw new NotImplementedException($"Rendering '{rendering.Item1}' is not known");
                }
            }
            this.fGrapher.SaveAll();
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
