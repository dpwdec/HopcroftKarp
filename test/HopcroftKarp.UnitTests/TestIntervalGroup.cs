using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace HopcroftKarp.UnitTests
{
    public class IntervalGroupTests
    {
        [Test]
        public void TestIntervalGroupRegular()
        {
            var x = new List<int> { 1, 2, 3, 4, 5, 6 };
            var y = x.IntervalGroup(2);
            Assert.AreEqual(x, new List<int> { 1, 2, 3, 4, 5, 6 });
            Assert.AreEqual(y, new List<List<int>> 
            {
                new List<int> { 1, 2 },
                new List<int> { 3, 4 },
                new List<int> { 5, 6 }
            });
        }

        [Test]
        public void TestIntervalGroupIrregular()
        {
            var x = new List<int> { 1, 2, 3, 4, 5, 6, 7 };
            var y = x.IntervalGroup(3);
            Assert.AreEqual(x, new List<int> { 1, 2, 3, 4, 5, 6, 7 });
            Assert.AreEqual(y, new List<List<int>> 
            {
                new List<int> { 1, 2, 3 },
                new List<int> { 4, 5, 6 },
                new List<int> { 7 }
            });
        }
    }
}