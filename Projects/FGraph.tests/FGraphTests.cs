using FGraph;
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
    }
}
