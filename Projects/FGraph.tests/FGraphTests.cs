using FGraph;
using SVGLib;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata;
using System.Text;
using Xunit;

namespace FGraph.Tests
{
    public class FGraphTests
    {
        [Fact]
        public void FindTest1()
        {
            FGrapher f = new FGrapher();
            f.Load("FindTest1.nodeGraph");
            Assert.True(f.TryGetNodeByName("Parent/Alpha", out GraphNodeWrapper dummy11));
            Assert.True(f.TryGetNodeByName("Parent/Beta", out GraphNodeWrapper dummy12));
            Assert.True(f.TryGetNodeByName("Main/Alpha", out GraphNodeWrapper dummy21));
            Assert.True(f.TryGetNodeByName("Main/Beta", out GraphNodeWrapper dummy22));
            Assert.True(f.TryGetNodeByName("Child/Alpha", out GraphNodeWrapper dummy31));
            Assert.True(f.TryGetNodeByName("Child/Beta", out GraphNodeWrapper dummy32));

            f.ProcessLinks();

            Assert.True(f.TryGetNodeByName("Main/Alpha", out GraphNodeWrapper mainAlpha));

            Assert.True(mainAlpha.ParentLinks.Count == 1);
            Assert.True(mainAlpha.ParentLinks[0].Node.NodeName == "Parent/Alpha");

            Assert.True(mainAlpha.ChildLinks.Count == 1);
            Assert.True(mainAlpha.ChildLinks[0].Node.NodeName == "Child/Beta");

            Assert.True(f.TryGetNodeByName("Main/Beta", out GraphNodeWrapper mainBeta));
            Assert.True(mainBeta.ParentLinks.Count == 0);
            Assert.True(mainBeta.ChildLinks.Count == 0);
        }

        [Fact]
        public void FindTest2()
        {
            FGrapher f = new FGrapher();
            f.Load("FindTest2.nodeGraph");
            f.ProcessLinks();

            Assert.True(f.TryGetNodeByName("Main/Alpha", out GraphNodeWrapper mainAlpha));

            Assert.True(mainAlpha.ParentLinks.Count == 2);
            Assert.True(mainAlpha.ChildLinks.Count == 2);

            Assert.True(f.TryGetNodeByName("Main/Beta", out GraphNodeWrapper mainBeta));
            Assert.True(mainBeta.ParentLinks.Count == 0);
            Assert.True(mainBeta.ChildLinks.Count == 0);
        }

        [Fact]
        public void FocusTest1()
        {
            FGrapher f = new FGrapher();
            f.Load("FocusTest1.nodeGraph");
            f.OutputDir = @"c:\Temp\FGraphTests";
            f.ProcessLinks();
            f.RenderFocusGraphs("FocusTest1.css");
            f.SaveAll();
        }
    }
}
