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
            Assert.True(String.Compare(f.BaseUrl, "http://hl7.org/fhir/us/breast-radiology", StringComparison.InvariantCulture) == 0);
            Assert.True(f.TryGetProfile("http://hl7.org/fhir/us/breast-radiology/StructureDefinition/BreastRadiologyComposition", out StructureDefinition sd));
            Assert.True(String.Compare(sd.Url, "http://hl7.org/fhir/us/breast-radiology/StructureDefinition/BreastRadiologyComposition", StringComparison.InvariantCulture) == 0);
            f.Load(TestFile("FindNodeByAnchorTest1.nodeGraph"));
            f.ProcessLinks();
            Debug.Assert(f.HasErrors == false);

            Assert.True(f.TryGetNodeByName("BreastRadiologyComposition", out GraphNode brComposition));
            Assert.True(brComposition.ParentLinks.Count == 0);
            Assert.True(brComposition.ChildLinks.Count == 4);

            GraphNode brCompReport = brComposition.ChildLinks[0].Node;
            Assert.True(brCompReport.DisplayName == "Report Section");
            Assert.True(brCompReport.ParentLinks.Count == 1);

            GraphNode brCompImpressions = brComposition.ChildLinks[1].Node;
            Assert.True(brCompImpressions.DisplayName == "Impressions Section");
            Assert.True(brCompImpressions.ParentLinks.Count == 1);

            GraphNode brCompfindingsRightBreast = brComposition.ChildLinks[2].Node;
            Assert.True(brCompfindingsRightBreast.DisplayName == "Findings/Right Breast/Section");
            Assert.True(brCompfindingsRightBreast.ParentLinks.Count == 1);
            
            GraphNode brCompfindingsLeftBreast = brComposition.ChildLinks[3].Node;
            Assert.True(brCompfindingsLeftBreast.DisplayName == "Findings/Left Breast/Section");
            Assert.True(brCompfindingsLeftBreast.ParentLinks.Count == 1);
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
        public void FocusRenderTest1()
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
