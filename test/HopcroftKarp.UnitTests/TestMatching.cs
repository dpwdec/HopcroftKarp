using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace HopcroftKarp.UnitTests
{
    public class MatchingTests
    {
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