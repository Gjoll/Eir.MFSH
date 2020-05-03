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

        public string OutputDir
        {
            get => this.outputDir;
            set => this.outputDir = Path.GetFullPath(value);
        }
        private string outputDir;

        Dictionary<String, StructureDefinition> profiles = new Dictionary<String, StructureDefinition>();
        Dictionary<String, ValueSet> valueSets = new Dictionary<String, ValueSet>();
        Dictionary<String, CodeSystem> codeSystems = new Dictionary<String, CodeSystem>();

        Dictionary<String, GraphNodeWrapper> graphNodesByName = new Dictionary<string, GraphNodeWrapper>();
        Dictionary<String, GraphNodeWrapper> graphNodesByElementId = new Dictionary<string, GraphNodeWrapper>();
        List<GraphLinkWrapper> graphLink = new List<GraphLinkWrapper>();

        List<SvgEditor> svgEditors = new List<SvgEditor>();

        public bool DebugFlag { get; set; } = false;

        public FGrapher()
        {
        }

        public bool TryGetNodeByName(String name, out GraphNodeWrapper node) => this.graphNodesByName.TryGetValue(name, out node);
        public bool TryGetNodeByElementId(String name, out GraphNodeWrapper node) => this.graphNodesByElementId.TryGetValue(name, out node);

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

            switch (domainResource)
            {
                case StructureDefinition sDef:
                    if (sDef.Snapshot == null)
                        SnapshotCreator.Create(sDef);
                    profiles.Add(sDef.Url.LastUriPart(), sDef);
                    break;
                case ValueSet valueSet:
                    valueSets.Add(valueSet.Url, valueSet);
                    break;
                case CodeSystem codeSystem:
                    codeSystems.Add(codeSystem.Url, codeSystem);
                    break;
            }
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
                        GraphNodeWrapper node = new GraphNodeWrapper(this, value);
                        this.graphNodesByName.Add(node.NodeName, node);
                        this.graphNodesByElementId.Add(node.ElementId, node);
                    }
                    break;

                case "graphLinkByReference":
                    {
                        GraphLinkByReferenceWrapper link = new GraphLinkByReferenceWrapper(this, value);
                        this.graphLink.Add(link);
                    }
                    break;

                case "graphLinkByName":
                    {
                        GraphLinkByNameWrapper link = new GraphLinkByNameWrapper(this, value);
                        this.graphLink.Add(link);
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
            ProcessLinks();
        }

        public void ProcessLinks()
        {
            foreach (GraphLinkWrapper link in this.graphLink)
                ProcessLink(link);
        }

        List<GraphNodeWrapper> FindNamedNodes(String name)
        {
            List<GraphNodeWrapper> retVal = new List<GraphNodeWrapper>();
            Regex r = new Regex(name);
            foreach (GraphNodeWrapper graphNode in this.graphNodesByName.Values)
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

                case GraphLinkByReferenceWrapper linkByRef:
                    ProcessLink(linkByRef);
                    break;

                default:
                    throw new NotImplementedException($"Unimplemented link type.");
            }
        }

        void ProcessLink(GraphLinkByReferenceWrapper link)
        {
            void CreateLink(GraphNodeWrapper sourceNode,
                String elementId)
            {
                if (sourceNode.ElementId.FirstPathPart() != elementId.FirstPathPart())
                {
                    this.ConversionError("FGrapher",
                        "ProcessLink",
                        $"Invalid reference element id. ElementId '{elementId}' is not an element of source profile '{sourceNode.ElementId.FirstPathPart()}");
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
                    GraphNodeWrapper targetNode = new GraphNodeWrapper(this);
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
                    GraphNodeWrapper targetNode = new GraphNodeWrapper(this);
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
                                String profileName = targetRef.LastUriPart();
                                if (this.TryGetNodeByElementId(profileName, out GraphNodeWrapper targetNode) == false)
                                {
                                    this.ConversionError("FGrapher",
                                        "FindElementDefinition",
                                        $"Can not find profile '{profileName}' referenced in annotation source.");
                                    return;
                                }

                                sourceNode.AddChild(link, link.Depth, targetNode);
                                targetNode.AddParent(link, link.Depth, sourceNode);
                            }
                            break;
                    }
                }
            }

            List<GraphNodeWrapper> sources = FindNamedNodes(link.Source);

            foreach (GraphNodeWrapper sourceNode in sources)
            {
                // make absolute element id.
                String elementId = link.ElementId;
                if (String.IsNullOrEmpty(elementId))
                    elementId = sourceNode.ElementId;
                else if (elementId.StartsWith("."))
                    elementId = sourceNode.ElementId + elementId;
                CreateLink(sourceNode, elementId);
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
