using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace HopcroftKarp.UnitTests
{
    public class HopcroftKarpTests
    {
        [Test]
        public void TestTwoNodeGraph()
        {
            var graph = new BipartiteGraph(
                new Dictionary<int, List<int>>
                {
                    { 0, new List<int> { 1 } },
                }
            );

            var matching = HopcroftKarpMatching.Run(graph);

            var expected = new Matching
            {
                Pairs = new List<(Node, Node)>
                {
                    (graph.Left[0], graph.Right[0])
                }
            };

            Assert.AreEqual(expected.Pairs, matching.Pairs);
        }

        [Test]
        public void TestFourNodeGraph()
        {
            var graph = new BipartiteGraph(
                new Dictionary<int, List<int>>
                {
                    { 0, new List<int> { 2 } },
                    { 1, new List<int> { 3 } }
                }
            );

            var matching = HopcroftKarpMatching.Run(graph);

            var expected = new Matching
            {
                Pairs = new List<(Node, Node)>
                {
                    (graph.Left[0], graph.Right[0]),
                    (graph.Left[1], graph.Right[1]),
                }
            };

            Assert.AreEqual(expected.Pairs, matching.Pairs);
        }


        // found a bug :<
        // test this graph configuration with bfs directly to make sure its correct
        [Test]
        public void TestSixNodeGraph()
        {
            var graph = new BipartiteGraph(
                new Dictionary<int, List<int>>
                {
                    { 0, new List<int> { 3, 4 } },
                    { 1, new List<int> { 3, 4, 5 } },
                    { 2, new List<int> { 4 } }
                }
            );

            var matching = HopcroftKarpMatching.Run(graph);

            var expected = new Matching
            {
                Pairs = new List<(Node, Node)>
                {
                    (graph.Left[0], graph.Right[0]),
                    (graph.Left[2], graph.Right[1]),
                    (graph.Left[1], graph.Right[2]),
                }
            };

            Assert.AreEqual(expected.Pairs, matching.Pairs);
        }
    }
}