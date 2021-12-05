using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace HopcroftKarp.UnitTests
{
    public class BipartiteGraphTests
    {
        [Test]
        public void TestGraphInitialization()
        {
            var graph = new BipartiteGraph(
                new Dictionary<int, List<int>>
                {
                    { 0, new List<int> { 3, 4 } },
                    { 1, new List<int> { 4 } },
                    { 2, new List<int> { 5 } },
                }
            );

            Assert.AreEqual(3, graph.Left.Count);
            Assert.AreEqual(3, graph.Right.Count);

            Assert.IsTrue(graph.Left[0].Connections.Contains(graph.Right[0]));
            Assert.IsTrue(graph.Left[0].Connections.Contains(graph.Right[1]));
            Assert.IsTrue(graph.Left[1].Connections.Contains(graph.Right[1]));
            Assert.IsTrue(graph.Left[2].Connections.Contains(graph.Right[2]));
        }
    }
}