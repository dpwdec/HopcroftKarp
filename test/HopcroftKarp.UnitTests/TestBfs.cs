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
        public void TestBfsUnmatchedGraph()
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
        public void TestBfsPartialMatchedGraph()
        {
            var a = new Node("a");
            var b = new Node("b");
            var c = new Node("c");
            var d = new Node("d");
            var e = new Node("e");
            var f = new Node("f");

            a.Connections.Add(d);
            a.Connections.Add(e);

            b.Connections.Add(e);

            c.Connections.Add(f);
            c.Connections.Add(d);

            d.Connections.Add(a);
            d.Connections.Add(c);

            e.Connections.Add(b);
            e.Connections.Add(a);

            f.Connections.Add(c);

            var graph = new BipartiteGraph()
            {
                Left = new List<Node>() { a, b, c },
                Right = new List<Node>() { d, e, f }
            };

            var matching = new Matching();
            matching.Pairs.Add((c, d));
            matching.Pairs.Add((a, e));

            var layers = HopcroftKarpMatching.Bfs(graph, matching);

            Console.WriteLine();

            var expected = new List<HashSet<Node>>()
            {
                new HashSet<Node> { b },
                new HashSet<Node> { e },
                new HashSet<Node> { a },
                new HashSet<Node> { d },
                new HashSet<Node> { c },
                new HashSet<Node> { f }
            };

            Assert.AreEqual(expected, layers);
        }

        [Test]
        public void TestBfsDualMatchLevels()
        {
            var a = new Node("a");
            var b = new Node("b");
            var c = new Node("c");
            var d = new Node("d");
            var e = new Node("e");
            var f = new Node("f");
            var g = new Node("g");
            var h = new Node("h");

            a.Connections.Add(d);

            b.Connections.Add(d);
            b.Connections.Add(f);

            c.Connections.Add(e);
            c.Connections.Add(h);

            g.Connections.Add(e);

            d.Connections.Add(a);
            d.Connections.Add(b);

            e.Connections.Add(c);
            e.Connections.Add(g);

            f.Connections.Add(b);

            h.Connections.Add(c);

            var graph = new BipartiteGraph()
            {
                Left = new List<Node>() { a, b, c, g },
                Right = new List<Node>() { d, e, f, h }
            };

            var matching = new Matching();
            matching.Pairs.Add((b, d));
            matching.Pairs.Add((c, e));

            var layers = HopcroftKarpMatching.Bfs(graph, matching);

            Console.WriteLine();

            var expected = new List<HashSet<Node>>()
            {
                new HashSet<Node> { a, g },
                new HashSet<Node> { d, e },
                new HashSet<Node> { b, c },
                new HashSet<Node> { f, h },
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