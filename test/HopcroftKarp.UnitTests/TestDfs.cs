using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace HopcroftKarp.UnitTests
{
    public class DfsTests
    {
        [Test]
        public void TestSingleBranchDfs()
        {
            var graph = new BipartiteGraph(
                new Dictionary<int, List<int>>
                {
                    { 0, new List<int> { 3, 4 } },
                    { 1, new List<int> { 4 } },
                    { 2, new List<int> { 3, 4, 5 } },
                }
            );

            var layers = new List<HashSet<Node>>
            {
                new HashSet<Node> { graph.Left[0] },  // 0
                new HashSet<Node> { graph.Right[0] }, // 3
                new HashSet<Node> { graph.Left[2] },  // 2
                new HashSet<Node> { graph.Right[1] }  // 4
            };

            var matching = new Matching()
            {
                Pairs = new List<(Node, Node)>
                {
                    (graph.Right[0], graph.Left[2])
                }
            };

            var path = HopcroftKarpMatching.Dfs(
                layers, 
                graph.Right[1], 
                layers.Count - 1, 
                matching, 
                new List<Node>()
            );

            var expected = new List<Node>
            {
                graph.Left[0],  // 0
                graph.Right[0], // 3
                graph.Left[2], // 2
                graph.Right[1] // 4
            };

            Assert.AreEqual(expected, path);
        }

        [Test]
        public void TestMultiBranchDfs()
        {
            var graph = new BipartiteGraph(
                new Dictionary<int, List<int>>
                {
                    { 0, new List<int> { 4 } },
                    { 1, new List<int> { 4, 6 } },
                    { 2, new List<int> { 5, 7 } },
                    { 3, new List<int> { 5 } }
                }
            );

            var layers = new List<HashSet<Node>>
            {
                new HashSet<Node> { graph.Left[0], graph.Left[3] },  // 0, 2
                new HashSet<Node> { graph.Right[0], graph.Right[2] }, // 4, 5
                new HashSet<Node> { graph.Left[1], graph.Left[2] },  // 1, 2
                new HashSet<Node> { graph.Right[1], graph.Right[3] }  // 6, 7
            };

            var matching = new Matching()
            {
                Pairs = new List<(Node, Node)>
                {
                    (graph.Left[1], graph.Right[0]),
                    (graph.Left[2], graph.Right[1])
                }
            };

            var path = HopcroftKarpMatching.Dfs(
                layers, 
                graph.Right[3], 
                layers.Count - 1, 
                matching, 
                new List<Node>()
            );

            var expected = new List<Node>
            {
                graph.Left[3],  // 3
                graph.Right[2], // 5
                graph.Left[2], // 2
                graph.Right[3] // 7
            };

            Assert.AreEqual(expected, path);
        }

        [Test]
        public void TestAlternateMultiBranchDfs()
        {
            var graph = new BipartiteGraph(
                new Dictionary<int, List<int>>
                {
                    { 0, new List<int> { 4 } },
                    { 1, new List<int> { 4, 6 } },
                    { 2, new List<int> { 5, 7 } },
                    { 3, new List<int> { 5 } }
                }
            );

            var layers = new List<HashSet<Node>>
            {
                new HashSet<Node> { graph.Left[0], graph.Left[3] },  // 0, 2
                new HashSet<Node> { graph.Right[0], graph.Right[2] }, // 4, 5
                new HashSet<Node> { graph.Left[1], graph.Left[2] },  // 1, 2
                new HashSet<Node> { graph.Right[1], graph.Right[3] }  // 6, 7
            };

            var matching = new Matching()
            {
                Pairs = new List<(Node, Node)>
                {
                    (graph.Left[1], graph.Right[0]),
                    (graph.Left[2], graph.Right[1])
                }
            };

            var path = HopcroftKarpMatching.Dfs(
                layers, 
                graph.Right[1],
                layers.Count - 1, 
                matching, 
                new List<Node>()
            );

            var expected = new List<Node>
            {
                graph.Left[0],  // 0
                graph.Right[0], // 4
                graph.Left[1], // 1
                graph.Right[1] // 6
            };

            Assert.AreEqual(expected, path);
        }
    }
}