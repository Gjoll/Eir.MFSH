using Eir.DevTools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;

namespace FGraph
{
    class FGrapher : ConverterBase
    {
        public string GraphName { get; set; }

        public string OutputDir
        {
            get => this.outputDir;
            set => this.outputDir = Path.GetFullPath(value);
        }
        private string outputDir;

        Dictionary<String, GraphNode> graphNodes = new Dictionary<string, GraphNode>();
        List<GraphLinkByName> graphLinkByNames = new List<GraphLinkByName>();

        public bool DebugFlag { get; set; } = false;

        void LoadDir(String path)
        {
            foreach (String subDir in Directory.GetDirectories(path))
                LoadDir(subDir);
            foreach (String file in Directory.GetFiles(path))
                LoadFile(file);
        }

        void LoadFile(String path)
        {
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                dynamic array = JsonConvert.DeserializeObject(json);

                foreach (dynamic item in array)
                    LoadItem(item);
            }
        }

        void LoadItem(dynamic item)
        {
            JObject j = item as JObject;
            foreach (KeyValuePair<String, JToken> kvp in j)
                LoadItem(kvp.Key, kvp.Value);
        }


        void LoadItem(String type, dynamic value)
        {
            if (value.graphName != this.GraphName)
                return;

            switch (type)
            {
                case "graphNode":
                    GraphNode node = new GraphNode(value);
                    this.graphNodes.Add(node.NodeName, node);
                    break;

                case "graphLinkByName":
                    GraphLinkByName link = new GraphLinkByName(value);
                    this.graphLinkByNames.Add(link);
                    break;

                default:
                    this.ConversionError("FGrapher",
                        "Load",
                        $"unknown graph item '{type}'");
                    return;
            }
        }

        public void Load(String path)
        {
            path = Path.GetFullPath(path);
            if (Directory.Exists(path) == true)
                LoadDir(path);
            else if (File.Exists(path))
                LoadFile(path);
            else
            {
                this.ConversionError("FGrapher",
                    "Load",
                    $"{path} not found");
            }
        }

        public void Process()
        {

        }
    }
}
