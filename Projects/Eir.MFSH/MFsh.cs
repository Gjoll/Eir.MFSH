using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Antlr4.Runtime.Tree;
using Eir.DevTools;
using Eir.MFSH;

namespace Eir.MFSH
{
    public class MFsh : ConverterBase
    {
        public bool DebugFlag { get; set; } = true;
        MFshManager mgr;

        public string BaseInputDir
        {
            get => this.baseInputDir;
            set => this.baseInputDir = Path.GetFullPath(value);
        }
        private string baseInputDir;

        public string BaseOutputDir
        {
            get => this.baseOutputDir;
            set => this.baseOutputDir = Path.GetFullPath(value);
        }
        private string baseOutputDir;

        public List<String> Paths = new List<string>();

        // Keep track of include files so we dont end up in recursive loop.
        List<String> sources = new List<string>();

        public const String FSHSuffix = ".fsh";
        public const String MFSHSuffix = ".mfsh";

        public String BaseUrl { get; set; }

        public VariablesBlock GlobalVars = new VariablesBlock();
        public Dictionary<String, FileData> FileItems =
            new Dictionary<String, FileData>();

        public MFsh()
        {
            this.mgr = new MFshManager(this);
        }

        public StackFrame Parse(String fshText,
            String sourceName,
            String localDir)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Process single file.
        /// </summary>
        public void LoadFile(String path)
        {
            const String fcn = "ProcessFile";

            if (path.StartsWith(BaseInputDir) == false)
                throw new Exception("Internal error. Path does not start with correct base path");

            this.ConversionInfo(this.GetType().Name, fcn, $"Loading file {path}");
            String fshText = File.ReadAllText(path);
            this.mgr.ParseOne(fshText, Path.GetFileName(path), "");

            //String baseRPath = path.Substring(BaseInputDir.Length);
            //if (baseRPath.StartsWith("\\"))
            //    baseRPath = baseRPath.Substring(1);
            //Int32 extensionIndex = baseRPath.LastIndexOf('.');
            //if (extensionIndex > 0)
            //    baseRPath = baseRPath.Substring(0, extensionIndex);

            //String baseName = Path.GetFileName(baseRPath);
            //String baseDir = Path.GetDirectoryName(baseRPath);

            //this.GlobalVars.Set("%BasePath%", baseRPath);
            //this.GlobalVars.Set("%BaseDir%", baseDir);
            //this.GlobalVars.Set("%BaseName%", baseName);
            //this.GlobalVars.Set("%SavePath%", $"{baseRPath}{MFsh.FSHSuffix}");

            //StackFrame frame = this.Parse(fshText,
            //    baseName,
            //    Path.GetDirectoryName(path));

            //void SaveData(FileData fileData)
            //{
            //    fileData.ProcessVariables(this.GlobalVars);
            //    if (this.FileItems.TryGetValue(fileData.RelativePath, out FileData existingReDir) == true)
            //        existingReDir.AppendText(fileData.Text());
            //    else
            //        this.FileItems.Add(fileData.RelativePath, fileData);
            //}

            //SaveData(frame.Data);
            //foreach (FileData reDir in frame.Redirections)
            //    SaveData(reDir);
        }

        /// <summary>
        /// Process all files in indicated dir and sub dirs.
        /// </summary>
        public void Process()
        {
            throw new NotImplementedException();
            if (String.IsNullOrEmpty(this.BaseUrl) == true)
                throw new Exception($"BaseUrl not set");

            foreach (String path in this.Paths)
            {
                try
                {
                    if (Directory.Exists(path))
                        this.LoadDir(path);
                    else if (File.Exists(path))
                        this.LoadFile(path);
                    else
                        throw new Exception($"Path {path} does not exist");
                }
                catch (Exception err)
                {
                    String fullMsg = $"Error loading {Path.GetFileName(path)}. '{err.Message}'";
                    this.ConversionError("mfsh", "ProcessInclude", fullMsg);
                }
            }
        }

        void LoadDir(String path,
            String filter = MFsh.MFSHSuffix)
        {
            const String fcn = "AddDir";

            this.ConversionInfo(this.GetType().Name, fcn, $"Loading directory {path}, filter {filter}");
            foreach (String subDir in Directory.GetDirectories(path))
                this.LoadDir(subDir, filter);

            foreach (String file in Directory.GetFiles(path, $"*{MFsh.MFSHSuffix}"))
                this.LoadFile(file);
        }

        /// <summary>
        /// Write all files back to disk.
        /// </summary>
        public void SaveAll()
        {
            const String fcn = "SaveAll";

            void Save(String outputPath, String text)
            {
                text = text.Trim();
                if (text.Length == 0)
                {
                    if (File.Exists(outputPath))
                        File.Delete(outputPath);
                    return;
                }

                String dir = Path.GetDirectoryName(outputPath);
                if (Directory.Exists(dir) == false)
                    Directory.CreateDirectory(dir);

                if (FileTools.WriteModifiedText(outputPath, text) == true)
                    this.ConversionInfo(this.GetType().Name,
                        fcn,
                        $"Saving {Path.GetFileName(outputPath)}");
            }

            this.BaseOutputDir = Path.GetFullPath(BaseOutputDir);
            this.ConversionInfo(this.GetType().Name, fcn, $"Saving all processed files");
            foreach (FileData f in this.FileItems.Values)
            {
                String outputPath = Path.Combine(BaseOutputDir, f.RelativePath);
                string outText = f.SaveText();
                Save(outputPath, outText);
            }
        }
    }
}
