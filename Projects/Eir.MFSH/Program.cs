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

namespace Eir.MFSH
{
    class Program
    {
        class Options
        {
            public class Clean
            {
                public String path { get; set; }
                public String filter { get; set; }
            };

            public class Define
            {
                public String name { get; set; }
                public String value{ get; set; }
            };

            public String baseInputDir { get; set; }
            public String baseOutputDir { get; set; }
            public String fragDir { get; set; }
            public String fragTemplatePath { get; set; }
            public String baseUrl { get; set; }
            public Clean[] cleanDirs { get; set; }
            public String[] mfshPaths { get; set; }
            public Define[] defines { get; set; }
        }

        Options options;
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
            this.options = JsonConvert.DeserializeObject<Options>(json);
        }
        void ParseArguments(String[] args)
        {
            switch (args.Length)
            {
                case 0:
                    this.ParseCommands("mfsh.json");
                    break;
                case 1:
                    this.ParseCommands(args[0]);
                    break;
                default:
                    throw new Exception($"Unexpected parameters");
            }
        }

        bool Process()
        {
            if (this.options.baseInputDir == null)
                throw new Exception("Missing 'baseInputDir' option setting");
            this.mfsh.BaseInputDir = Path.GetFullPath(this.options.baseInputDir);

            if (this.options.baseOutputDir == null)
                throw new Exception("Missing 'baseOutputDir' option setting");
            this.mfsh.BaseOutputDir = Path.GetFullPath(this.options.baseOutputDir);

            this.mfsh.FragDir = Path.GetFullPath(this.options.fragDir);
            if (String.IsNullOrEmpty(this.options.fragTemplatePath) == false)
                this.mfsh.FragTemplatePath = Path.GetFullPath(this.options.fragTemplatePath);

            if (this.options.baseUrl == null)
                throw new Exception("Missing 'baseUrl' option setting");
            this.mfsh.BaseUrl = this.options.baseUrl;

            foreach (Options.Clean cleanDir in this.options.cleanDirs)
                this.mfsh.FileClean(cleanDir.path, cleanDir.filter);

            foreach (Options.Define define in this.options.defines)
                this.mfsh.GlobalVars.Set(define.name, define.value);

            foreach (String mfshPath in this.options.mfshPaths)
                this.mfsh.Load(mfshPath);

            if (this.mfsh.HasErrors == true)
                return false;

            this.mfsh.Process();
            if (this.mfsh.HasErrors == true)
                return false;

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
