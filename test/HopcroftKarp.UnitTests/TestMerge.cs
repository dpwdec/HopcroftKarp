using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace HopcroftKarp.UnitTests
{
    public class MergeTests
    {
        [Test]
        public void TestMergeExtension()
        {
            var x = new List<int> { 2, 3, 4 };
            var y = new List<int>() { 1 }.Merge(x);
            Assert.AreEqual(new List<int>() { 1, 2, 3, 4 }, y);
        }
    }
}