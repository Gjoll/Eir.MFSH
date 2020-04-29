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
    public class FGrapher : ConverterBase
    {
        public string GraphName { get; set; } = null;

        public string OutputDir
        {
            get => this.outputDir;
            set => this.outputDir = Path.GetFullPath(value);
        }
        private string outputDir;

        Dictionary<String, GraphNode> graphNodes = new Dictionary<string, GraphNode>();
        List<GraphLinkByName> graphLinkByNames = new List<GraphLinkByName>();

        public bool DebugFlag { get; set; } = false;

        public FGrapher()
        {
        }

        public bool TryGetNodeByName(String name, out GraphNode node) => this.graphNodes.TryGetValue(name, out node);

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

        public void LoadItem(dynamic item)
        {
            JObject j = item as JObject;
            foreach (KeyValuePair<String, JToken> kvp in j)
                LoadItem(kvp.Key, kvp.Value);
        }


        public void LoadItem(String type, dynamic value)
        {
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
        public void RenderFocusGraphs()
        {
            foreach (GraphNode node in this.graphNodes.Values)
            {
                this.RenderGraph(node, $"focus/{node.NodeName.FirstSlashPart()}");
            }
        }

        void RenderGraph(GraphNode node,
            String traversalName)
        {
            GraphItemGroup focusGroup = new GraphItemGroup();
            focusGroup.Nodes.Add(node);

            RenderGroupParents(node, focusGroup, traversalName);
        }

        void RenderGroupParents(GraphNode node,
            GraphItemGroup focusGroup,
            String traversalName)
        {
            HashSet<GraphNode> parentNodes = new HashSet<GraphNode>();
            parentNodes.Add(node);

            GraphItemGroup parentGroup = new GraphItemGroup();
            foreach (GraphNode.Link parentLink in node.ParentLinks)
            {
                if (
                    (parentLink.TraversalName == traversalName) &&
                    (parentNodes.Contains(parentLink.Node) == false)
                    )
                {
                    parentNodes.Add(parentLink.Node);  // dont add same node twice...
                    parentGroup.Nodes.Add(parentLink.Node);
                }
            }

            if (parentGroup.Nodes.Count == 0)
                return;
        }

        public void Process()
        {
            ProcessLinks();
        }

        public void ProcessLinks()
        {
            foreach (GraphLinkByName link in this.graphLinkByNames)
                ProcessLink(link);
        }

        List<GraphNode> FindNamedNodes(String name)
        {
            List<GraphNode> retVal = new List<GraphNode>();
            Regex r = new Regex(name);
            foreach (GraphNode graphNode in graphNodes.Values)
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

        void ProcessLink(GraphLinkByName link)
        {
            List<GraphNode> sources = FindNamedNodes(link.Source);
            List<GraphNode> targets = FindNamedNodes(link.Target);
            if ((sources.Count > 1) && (targets.Count > 1))
            {
                this.ConversionError("FGrapher",
                    "ProcessLink",
                    $"Many to many link not supported. {link.Source}' <--> {link.Target}");
            }

            foreach (GraphNode sourceNode in sources)
            {
                foreach (GraphNode targetNode in targets)
                {
                    sourceNode.AddChild(link.TraversalName, targetNode);
                    targetNode.AddParent(link.TraversalName, sourceNode);
                }
            }
        }

    }
}
