using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace HopcroftKarp.UnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestBfsOnUnmatchGraph()
        {
            var a = new Node("foo");
            var b = new Node("bar");
            var c = new Node("baz");
            var d = new Node("foo");
            var e = new Node("bar");
            var f = new Node("baze");

            a.Connections.Add(d);
            b.Connections.Add(e);
            c.Connections.Add(f);
            d.Connections.Add(a);
            e.Connections.Add(b);
            f.Connections.Add(c);

            var graph = new BipartiteGraph()
            {
                Left = new List<Node>() { a, b, c },
                Right = new List<Node>() { d, e, f }
            };

            var matching = new Matching();

            var layers = HopcroftKarpMatching.Bfs(graph, matching);

            Console.WriteLine();

            var expected = new List<HashSet<Node>>()
            {
                new HashSet<Node> { a, b, c },
                new HashSet<Node> { d, e, f }
            };

            Assert.AreEqual(expected, layers);
        }

        [Test]
        public void TestMatchingNonEmptyContains()
        {
            var matching = new Matching();
            var a = new Node("a");
            var b = new Node("b");
            var c = new Node("c");
            matching.Pairs.Add((a, b));
            Assert.AreEqual(matching.Contains(a), true);
            Assert.AreEqual(matching.Contains(c), false);
        }
    }
}