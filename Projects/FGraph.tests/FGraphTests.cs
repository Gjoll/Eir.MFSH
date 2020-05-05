using FGraph;
using SVGLib;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata;
using System.Text;
using Eir.DevTools;
using Hl7.Fhir.Model;
using Xunit;

namespace FGraph.Tests
{
    public class FGraphTests
    {
        public String TestFile(string s) => Path.Combine("TestFiles", s);

        [Fact]
        public void BaseUrlTest()
        {
            String url = "http://hl7.org/fhir/us/breast-radiology/StructureDefinition/BreastRadiologyComposition";
            String baseUrl = url.FhirBaseUrl();
            Assert.True(String.Compare(baseUrl, "http://hl7.org/fhir/us/breast-radiology") == 0);
        }

        [Fact]
        public void FindNodeByAnchorTest1()
        {
            FGrapher f = new FGrapher();
            f.LoadResources(TestFile("profiles"));
            Assert.True(String.Compare(f.BaseUrl, "http://hl7.org/fhir/us/breast-radiology") == 0);
            Assert.True(f.TryGetProfile("http://hl7.org/fhir/us/breast-radiology/StructureDefinition/BreastRadiologyComposition", out StructureDefinition sd));
            Assert.True(String.Compare(sd.Url, "http://hl7.org/fhir/us/breast-radiology/StructureDefinition/BreastRadiologyComposition", StringComparison.InvariantCulture) == 0);
        }

        [Fact]
        public void FindNodeByName1()
        {
            FGrapher f = new FGrapher();
            f.Load(TestFile("FindNodeByName1.nodeGraph"));
            Assert.True(f.TryGetNodeByName("Parent/Alpha", out GraphNode dummy11));
            Assert.True(f.TryGetNodeByName("Parent/Beta", out GraphNode dummy12));
            Assert.True(f.TryGetNodeByName("Main/Alpha", out GraphNode dummy21));
            Assert.True(f.TryGetNodeByName("Main/Beta", out GraphNode dummy22));
            Assert.True(f.TryGetNodeByName("Child/Alpha", out GraphNode dummy31));
            Assert.True(f.TryGetNodeByName("Child/Beta", out GraphNode dummy32));

            f.ProcessLinks();

            Assert.True(f.TryGetNodeByName("Main/Alpha", out GraphNode mainAlpha));

            Assert.True(mainAlpha.ParentLinks.Count == 1);
            Assert.True(mainAlpha.ParentLinks[0].Node.NodeName == "Parent/Alpha");

            Assert.True(mainAlpha.ChildLinks.Count == 1);
            Assert.True(mainAlpha.ChildLinks[0].Node.NodeName == "Child/Beta");

            Assert.True(f.TryGetNodeByName("Main/Beta", out GraphNode mainBeta));
            Assert.True(mainBeta.ParentLinks.Count == 0);
            Assert.True(mainBeta.ChildLinks.Count == 0);
        }

        [Fact]
        public void FindNodeByName2()
        {
            FGrapher f = new FGrapher();
            f.Load(TestFile("FindNodeByName2.nodeGraph"));
            f.ProcessLinks();

            Assert.True(f.TryGetNodeByName("Main/Alpha", out GraphNode mainAlpha));

            Assert.True(mainAlpha.ParentLinks.Count == 2);
            Assert.True(mainAlpha.ChildLinks.Count == 2);

            Assert.True(f.TryGetNodeByName("Main/Beta", out GraphNode mainBeta));
            Assert.True(mainBeta.ParentLinks.Count == 0);
            Assert.True(mainBeta.ChildLinks.Count == 0);
        }

        [Fact]
        public void usRenderTest1()
        {
            FGrapher f = new FGrapher();
            f.Load(TestFile("FocusRenderTest1.nodeGraph"));
            f.OutputDir = @"c:\Temp\FGraphTests";
            f.ProcessLinks();
            f.RenderFocusGraphs(TestFile("FocusRenderTest1.css"));
            f.SaveAll();
        }
    }
}
