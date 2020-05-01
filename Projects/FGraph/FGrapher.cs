using Eir.DevTools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace FGraph
{
    public partial class FGrapher : ConverterBase
    {
        public string GraphName { get; set; } = null;

        public string OutputDir
        {
            get => this.outputDir;
            set => this.outputDir = Path.GetFullPath(value);
        }
        private string outputDir;

        Dictionary<String, GraphNodeWrapper> graphNodes = new Dictionary<string, GraphNodeWrapper>();
        List<GraphLinkWrapper> graphLink = new List<GraphLinkWrapper>();

        List<SvgEditor> svgEditors = new List<SvgEditor>();

        public bool DebugFlag { get; set; } = false;

        public FGrapher()
        {
        }

        public bool TryGetNodeByName(String name, out GraphNodeWrapper node) => this.graphNodes.TryGetValue(name, out node);

        public bool IsGraphMember(String graphName)
        {
            if (String.IsNullOrEmpty(this.GraphName))
                return true;
            if (String.IsNullOrEmpty(graphName))
                return true;
            return String.Compare(this.GraphName, graphName) == 0;
        }

        void LoadDir(String path)
        {
            foreach (String subDir in Directory.GetDirectories(path))
                LoadDir(subDir);
            foreach (String file in Directory.GetFiles(path, "*.nodeGraph"))
                LoadFile(file);
        }

        void LoadFile(String path)
        {
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                JArray array = JsonConvert.DeserializeObject<JArray>(json);

                foreach (var item in array)
                    LoadItem(item);
            }
        }

        public void LoadItem(JToken item)
        {
            JObject j = item as JObject;
            foreach (KeyValuePair<String, JToken> kvp in j)
                LoadItem(kvp.Key, kvp.Value);
        }


        public void LoadItem(String type, JToken value)
        {
            switch (type)
            {
                case "graphNode":
                    GraphNodeWrapper node = new GraphNodeWrapper(value);
                    this.graphNodes.Add(node.NodeName, node);
                    break;

                case "graphLinkByName":
                    GraphLinkByNameWrapper link = new GraphLinkByNameWrapper(value);
                    this.graphLink.Add(link);
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
            if (String.IsNullOrEmpty(this.OutputDir) == true)
                throw new Exception($"Output not set");
            ProcessLinks();
        }

        public void ProcessLinks()
        {
            foreach (GraphLinkByNameWrapper link in this.graphLink)
                ProcessLink(link);
        }

        List<GraphNodeWrapper> FindNamedNodes(String name)
        {
            List<GraphNodeWrapper> retVal = new List<GraphNodeWrapper>();
            Regex r = new Regex(name);
            foreach (GraphNodeWrapper graphNode in graphNodes.Values)
            {
                if (r.IsMatch(graphNode.NodeName))
                    retVal.Add(graphNode);
            }
            if (retVal.Count == 0)
            {
                this.ConversionWarn("FGrapher",
                    "FindNamedNodes",
                    $"No nodes named '{name}' found");
            }

            return retVal;
        }

        void ProcessLink(GraphLinkWrapper link)
        {
            switch (link)
            {
                case GraphLinkByNameWrapper linkByName:
                    ProcessLink(linkByName);
                    break;

                default:
                    throw new NotImplementedException($"Unimplemented link type.");
            }
        }

        void ProcessLink(GraphLinkByNameWrapper link)
        {
            List<GraphNodeWrapper> sources = FindNamedNodes(link.Source);
            List<GraphNodeWrapper> targets = FindNamedNodes(link.Target);
            if ((sources.Count > 1) && (targets.Count > 1))
            {
                this.ConversionError("FGrapher",
                    "ProcessLink",
                    $"Many to many link not supported. {link.Source}' <--> {link.Target}");
            }

            foreach (GraphNodeWrapper sourceNode in sources)
            {
                foreach (GraphNodeWrapper targetNode in targets)
                {
                    sourceNode.AddChild(link, targetNode);
                    targetNode.AddParent(link, sourceNode);
                }
            }
        }

        public void SaveAll()
        {
            foreach (SvgEditor svgEditor in this.svgEditors)
            {
                svgEditor.Save($"{this.outputDir}\\{svgEditor.Name}.svg");
            }
        }
    }
}
