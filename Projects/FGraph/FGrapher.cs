using Eir.DevTools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Hl7.Fhir.Model;
using System.Globalization;
using Hl7.Fhir.Serialization;
using FhirKhit.Tools.R4;

namespace FGraph
{
    public partial class FGrapher : ConverterBase
    {
        public string GraphName { get; set; } = null;

        public string BaseUrl { get; set; }

        public string OutputDir
        {
            get => this.outputDir;
            set => this.outputDir = Path.GetFullPath(value);
        }
        private string outputDir;

        Dictionary<String, StructureDefinition> profiles = new Dictionary<String, StructureDefinition>();
        Dictionary<String, ValueSet> valueSets = new Dictionary<String, ValueSet>();
        Dictionary<String, CodeSystem> codeSystems = new Dictionary<String, CodeSystem>();

        Dictionary<String, GraphNode> graphNodesByName = new Dictionary<string, GraphNode>();
        Dictionary<GraphAnchor, GraphNode> graphNodesByAnchor = new Dictionary<GraphAnchor, GraphNode>();

        List<GraphLink> graphLinks = new List<GraphLink>();
        List<SvgEditor> svgEditors = new List<SvgEditor>();

        public bool DebugFlag { get; set; } = false;

        public FGrapher()
        {
        }

        public bool TryGetNodeByName(String name, out GraphNode node) => this.graphNodesByName.TryGetValue(name, out node);
        public bool TryGetNodeByAnchor(GraphAnchor anchor, out GraphNode node) => this.graphNodesByAnchor.TryGetValue(anchor, out node);


        public bool TryGetProfile(String url, out StructureDefinition sd) => this.profiles.TryGetValue(url, out sd);
        public bool TryGetValueSet(String url, out ValueSet vs) => this.valueSets.TryGetValue(url, out vs);
        public bool TryGetProfile(String url, out CodeSystem cs) => this.codeSystems.TryGetValue(url, out cs);


        public void LoadResources(String path)
        {
            path = Path.GetFullPath(path);
            if (Directory.Exists(path) == true)
                LoadResourceDir(path);
            else if (File.Exists(path))
                LoadResourceFile(path);
            else
            {
                this.ConversionError("FGrapher",
                    "LoadResources",
                    $"{path} not found");
            }
        }


        void LoadResourceDir(String path)
        {
            foreach (String subDir in Directory.GetDirectories(path))
                LoadResourceDir(subDir);
            foreach (String file in Directory.GetFiles(path, "*.json"))
                LoadResourceFile(file);
            foreach (String file in Directory.GetFiles(path, "*.xml"))
                LoadResourceFile(file);
        }


        void LoadResourceFile(String path)
        {
            DomainResource domainResource;
            switch (Path.GetExtension(path).ToUpper(CultureInfo.InvariantCulture))
            {
                case ".XML":
                    {
                        FhirXmlParser parser = new FhirXmlParser();
                        domainResource = parser.Parse<DomainResource>(File.ReadAllText(path));
                        break;
                    }

                case ".JSON":
                    {
                        FhirJsonParser parser = new FhirJsonParser();
                        domainResource = parser.Parse<DomainResource>(File.ReadAllText(path));
                        break;
                    }

                default:
                    throw new Exception($"Unknown extension for serialized fhir resource '{path}'");
            }

            String resourceUrl = null;
            switch (domainResource)
            {
                case StructureDefinition sDef:
                    if (sDef.Snapshot == null)
                        SnapshotCreator.Create(sDef);
                    resourceUrl = sDef.Url;
                    profiles.Add(sDef.Url, sDef);
                    break;

                case ValueSet valueSet:
                    resourceUrl = valueSet.Url;
                    valueSets.Add(valueSet.Url, valueSet);
                    break;

                case CodeSystem codeSystem:
                    resourceUrl = codeSystem.Url;
                    codeSystems.Add(codeSystem.Url, codeSystem);
                    break;
            }

            // We expect to only load resources all with the same base resoruce name.
            String rBaseUrl = resourceUrl.FhirBaseUrl();
            if (this.BaseUrl == null)
                this.BaseUrl = rBaseUrl;
            else if (String.Compare(this.BaseUrl, rBaseUrl, StringComparison.InvariantCulture) != 0)
                throw new Exception("Resource '{resourceUrl}' does not have base url '{this.BaseUrl}'");
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
                    {
                        GraphNode node = new GraphNode(this, value);
                        this.graphNodesByName.Add(node.NodeName, node);
                        if (node.Anchor != null)
                            this.graphNodesByAnchor.Add(node.Anchor, node);
                    }
                    break;

                case "graphLinkByReference":
                    {
                        GraphLinkByReference link = new GraphLinkByReference(this, value);
                        this.graphLinks.Add(link);
                    }
                    break;

                case "graphLinkByName":
                    {
                        GraphLinkByName link = new GraphLinkByName(this, value);
                        this.graphLinks.Add(link);
                    }
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
            if (String.IsNullOrEmpty(this.BaseUrl) == true)
                throw new Exception($"BaseUrl not set");
            ProcessLinks();
        }

        public void ProcessLinks()
        {
            foreach (GraphLink link in this.graphLinks)
                ProcessLink(link);
        }

        List<GraphNode> FindNamedNodes(String name)
        {
            List<GraphNode> retVal = new List<GraphNode>();
            Regex r = new Regex(name);
            foreach (GraphNode graphNode in this.graphNodesByName.Values)
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

        void ProcessLink(GraphLink link)
        {
            switch (link)
            {
                case GraphLinkByName linkByName:
                    ProcessLink(linkByName);
                    break;

                case GraphLinkByReference linkByRef:
                    ProcessLink(linkByRef);
                    break;

                default:
                    throw new NotImplementedException($"Unimplemented link type.");
            }
        }

        void ProcessLink(GraphLinkByReference link)
        {
            void CreateLink(GraphNode sourceNode,
                String elementId)
            {
                GraphAnchor anchor = sourceNode.Anchor;
                if (
                    (anchor != null) &&
                    (anchor.Url != elementId.FirstPathPart())
                    )
                {
                    this.ConversionError("FGrapher",
                        "ProcessLink",
                        $"Invalid reference element id. ElementId '{elementId}' is not an element of source profile '{anchor.Url}");
                    return;
                }

                ElementDefinition elementDiff = this.FindDiffElement(elementId);
                if (elementDiff == null)
                    return;

                ElementDefinition elementSnap = this.FindSnapElement(elementId);
                if (elementSnap == null)
                    return;

                if (elementDiff.Binding != null)
                {
                    GraphNode targetNode = new GraphNode(this);
                    targetNode.DisplayName = elementDiff.Binding.ValueSet.LastPathPart();
                    if (this.valueSets.TryGetValue(elementDiff.Binding.ValueSet, out ValueSet vs) == true)
                    {
                        targetNode.DisplayName = vs.Name;
                    }
                    targetNode.DisplayName += "/ValueSet";
                    targetNode.LhsAnnotationText = "bind";
                    sourceNode.AddChild(link, 0, targetNode);
                    targetNode.AddParent(link, 0, sourceNode);
                }

                if (elementDiff.Pattern != null)
                {
                    GraphNode targetNode = new GraphNode(this);
                    targetNode.LhsAnnotationText = "pattern";
                    sourceNode.AddChild(link, 0, targetNode);
                    targetNode.AddParent(link, 0, sourceNode);

                    switch (elementDiff.Pattern)
                    {
                        case CodeableConcept codeableConcept:
                            targetNode.DisplayName += elementDiff.Pattern.ToString();
                            break;

                        default:
                            targetNode.DisplayName += elementDiff.Pattern.ToString();
                            break;
                    }

                    this.ConversionWarn("FGrapher",
                        "ProcessLink",
                        $"ElementId '{elementId}' pattern reference is not implemented");
                }

                if (elementDiff.Fixed != null)
                {
                    this.ConversionWarn("FGrapher",
                        "ProcessLink",
                        $"ElementId '{elementId}' fixed reference is not implemented");
                }

                foreach (var typeRef in elementDiff.Type)
                {
                    switch (typeRef.Code)
                    {
                        case "Reference":
                            foreach (String targetRef in typeRef.TargetProfile)
                            {
                                sourceNode.RhsAnnotationText = $"{elementSnap.Min.Value}..{elementSnap.Max}";
                                GraphAnchor targetAnchor = new GraphAnchor(targetRef, null);
                                if (this.TryGetNodeByAnchor(targetAnchor, out GraphNode targetNode) == false)
                                {
                                    this.ConversionError("FGrapher",
                                        "FindElementDefinition",
                                        $"Can not find target '{targetRef}' referenced in annotation source.");
                                    return;
                                }

                                sourceNode.AddChild(link, link.Depth, targetNode);
                                targetNode.AddParent(link, link.Depth, sourceNode);
                            }
                            break;
                    }
                }
            }

            List<GraphNode> sources = FindNamedNodes(link.Source);

            foreach (GraphNode sourceNode in sources)
            {
                void Link()
                {
                    String linkElementId = link.ElementId;

                    if (
                        (String.IsNullOrEmpty(linkElementId)) ||
                        (linkElementId.StartsWith("."))
                    )
                    {
                        if (sourceNode.Anchor == null)
                            return;
                        GraphAnchor anchor = sourceNode.Anchor;
                        if (anchor == null)
                            return;
                        throw new NotImplementedException();
                        //$linkElementId = anchor.ElementId + anchor.ElementId;
                    }
                    CreateLink(sourceNode, linkElementId);
                }
                Link();
            }
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
                    sourceNode.AddChild(link, link.Depth, targetNode);
                    targetNode.AddParent(link, link.Depth, sourceNode);
                }
            }
        }

        public void SaveAll()
        {
            foreach (SvgEditor svgEditor in this.svgEditors)
            {
                String fileName = svgEditor.Name.Replace(":", "-");
                svgEditor.Save($"{this.outputDir}\\{fileName}.svg");
            }
        }

        public ElementDefinition FindSnapElement(String elementId)
        {
            const String fcn = "FindDiffElement";

            String profileName = elementId.FirstPathPart();
            if (this.profiles.TryGetValue(profileName, out var sDef) == false)
            {
                this.ConversionError("FGrapher",
                    fcn,
                    $"Can not find profile '{profileName}' referenced in annotation source.");
                return null;
            }

            ElementDefinition e = sDef.FindSnapElement(elementId);
            if (e == null)
            {
                this.ConversionError("FGrapher",
                    fcn,
                    $"Can not find profile 'element {elementId}' referenced in annotation source.");
                return null;
            }
            return e;
        }

        public ElementDefinition FindDiffElement(String elementId)
        {
            const String fcn = "FindDiffElement";

            String profileName = elementId.FirstPathPart();
            if (this.profiles.TryGetValue(profileName, out var sDef) == false)
            {
                this.ConversionError("FGrapher",
                    fcn,
                    $"Can not find profile '{profileName}' referenced in annotation source.");
                return null;
            }

            ElementDefinition e = sDef.FindDiffElement(elementId);
            if (e == null)
            {
                this.ConversionError("FGrapher",
                    fcn,
                    $"Can not find profile 'element {elementId}' referenced in annotation source.");
                return null;
            }
            return e;
        }
    }
}
