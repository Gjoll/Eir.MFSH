using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
                    case "baseInputDir":
                        this.mfsh.BaseInputDir = option.Value.Value<String>();
                        break;

                    case "baseOutputDir":
                        this.mfsh.BaseOutputDir = option.Value.Value<String>();
                        break;

                    case "baseUrl":
                        this.mfsh.BaseUrl = option.Value.Value<String>();
                        break;

                    case "defines":
                        foreach (Tuple<String, String> t in option.Value.GetTuples())
                            this.mfsh.GlobalVars.Set(t.Item1, t.Item2);
                        break;

                    case "includeDirs":
                        this.mfsh.IncludeDirs.AddRange(option.Value.GetStrings());
                        break;

                    case "mfshPaths":
                        this.mfsh.Paths.AddRange(option.Value.GetStrings());
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
                    ParseCommands("mfsh.json");
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
            if (String.IsNullOrEmpty(this.mfsh.BaseUrl) == true)
                throw new Exception($"BaseUrl not set (-u option)");

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
                String[] lines = e.ToString().Replace("\r", "").Split('\n');
                foreach (String line in lines)
                    Console.WriteLine($"Error: {line}");
                return -1;
            }
        }
    }
}
