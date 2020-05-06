using System;
using System.Collections.Generic;
using System.IO;
using Hl7.Fhir.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FGraph
{
    class Program
    {
        class Rendering
        {
            public String Name { get; set; }
            public String CssFile { get; set; }
        };

        FGrapher fGrapher;
        String inputDir = null;
        List<Rendering> renderings = new List<Rendering>();

        public Program()
        {
            this.fGrapher = new FGrapher();
            this.fGrapher.ConsoleLogging();
        }

        //void ParseArguments(String[] args)
        //{
        //    Int32 i = 0;

        //    String GetArg(String errorMsg)
        //    {
        //        if (i >= args.Length)
        //            throw new Exception($"Missing {errorMsg} parameter");

        //        String arg = args[i++].Trim();
        //        if (arg.Length > 0)
        //        {
        //            if ((arg[0] == '"') && (arg[arg.Length - 1] == '"'))
        //                arg = arg.Substring(1, arg.Length - 2);
        //        }

        //        return arg;
        //    }

        //    while (i < args.Length)
        //    {
        //        String arg = GetArg("arg").ToUpper();
        //        switch (arg.ToLower())
        //        {
        //            case "-r":
        //                this.fGrapher.LoadResources(GetArg(arg));
        //                break;

        //            case "-g":
        //            case "-graphname":
        //                if (this.fGrapher.GraphName != null)
        //                    throw new Exception($"{arg} option can only be used once.");
        //                this.fGrapher.GraphName = GetArg(arg);
        //                break;

        //            case "-o":
        //            case "-output":
        //                if (this.fGrapher.OutputDir != null)
        //                    throw new Exception($"{arg}  option can only be used once.");
        //                this.fGrapher.OutputDir = GetArg(arg);
        //                break;

        //            case "-t":
        //            case "-traversal":
        //                // traversal name, CSS path
        //                this.renderings.Add(new Tuple<string, string>(GetArg(arg), GetArg(arg)));
        //                break;

        //            case "-u":
        //            case "-url":
        //                if (this.fGrapher.BaseUrl!= null)
        //                    throw new Exception($"{arg}  option can only be used once.");
        //                this.fGrapher.BaseUrl = GetArg(arg);
        //                break;

        //            default:
        //                throw new Exception($"Unknown arg {arg}");
        //        }
        //    }
        //}

        void ParseCommands(String path)
        {
            String fullPath = Path.GetFullPath(path);
            if (File.Exists(path) == false)
                throw new Exception($"Options file {path} not found");
            String json = File.ReadAllText(path);
            JObject options = JsonConvert.DeserializeObject<JObject>(json); ;
            foreach (KeyValuePair<String, JToken> option in options)
            {
                switch (option.Key)
                {
                    case "traversals":
                        foreach (JProperty traversal in option.Value)
                        {
                            Rendering r = new Rendering();
                            r.Name = traversal.Name;
                            foreach (JObject rOption in traversal)
                            {
                                foreach (KeyValuePair<String, JToken> rOptionItem in rOption)
                                {
                                    switch (rOptionItem.Key)
                                    {
                                        case "cssFile":
                                            r.CssFile = rOptionItem.Value.Value<String>();
                                            break;
                                        default:
                                            throw new Exception($"Unknown option {rOptionItem.Key}");
                                    }
                                }
                            }
                        }
                        break;

                    case "outputPath":
                        this.fGrapher.OutputDir = option.Value.Value<String>();
                        break;

                    case "inputPath":
                        {
                            String inputPath = option.Value.Value<String>();
                            this.fGrapher.Load(inputPath);
                            if (Directory.Exists(inputPath))
                                this.inputDir = inputPath;
                            else
                                this.inputDir = Path.GetDirectoryName(inputPath);
                        }
                        break;


                    case "graphName":
                        this.fGrapher.GraphName = option.Value.Value<String>();
                        break;

                    case "resourcePaths":
                        foreach (String resourcePath in option.Value.GetStrings())
                            this.fGrapher.LoadResources(resourcePath);
                        break;

                    default:
                        throw new Exception($"Unknown option {option.Key}");
                }
            }
        }

        void ParseArguments(String[] args)
        {
            switch (args.Length)
            {
                case 0:
                    ParseCommands("fgraph.json");
                    break;
                case 1:
                    ParseCommands(args[0]);
                    break;
                default:
                    throw new Exception($"Unexpected parameters");
            }
        }

        bool Process()
        {
            this.fGrapher.Process();
            foreach (Rendering rendering in this.renderings)
            {
                bool Exists(String dir, ref String relativePath)
                {
                    String checkPath = Path.Combine(dir, relativePath);
                    if (File.Exists(checkPath) == false)
                        return false;
                    relativePath = checkPath;
                    return true;
                }

                String cssFile = rendering.CssFile;
                if (
                    (!Exists(Path.GetFullPath("."), ref cssFile)) &&
                    (!Exists(this.inputDir, ref cssFile))
                )
                    throw new Exception($"Css file '{cssFile}' not found");

                switch (rendering.Name.ToLower())
                {
                    case "focus":
                        this.fGrapher.RenderFocusGraphs(cssFile);
                        break;
                    default:
                        throw new NotImplementedException($"Rendering '{rendering.Name}' is not known");
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
