using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace HopcroftKarp.UnitTests
{
    public class MergeAugmentingPathTests
    {
        [Test]
        public void TestSingleDepthAugmentingPath()
        {
            var a = new Node("a");
            var b = new Node("b");

            var path = new List<Node>() { a, b };

            var matching = new Matching
            {
                Pairs = new List<(Node, Node)>()
            };

            var expected = new Matching
            {
                Pairs = new List<(Node, Node)>()
                {
                    (a, b)
                }
            };

             matching.MergeAugmentingPath(path);

             Assert.AreEqual(expected.Pairs, matching.Pairs);
        }

        [Test]
        public void TestMultiDepthAugmentingPath()
        {
            var a = new Node("a");
            var b = new Node("b");
            var c = new Node("c");
            var d = new Node("d");

            var path = new List<Node>() { a, b, c, d };

            var matching = new Matching
            {
                Pairs = new List<(Node, Node)>
                {
                    (b, c)
                }
            };

            var expected = new Matching
            {
                Pairs = new List<(Node, Node)>()
                {
                    (a, b),
                    (c, d)
                }
            };

             matching.MergeAugmentingPath(path);

             Assert.AreEqual(expected.Pairs, matching.Pairs);
        }
    }
}