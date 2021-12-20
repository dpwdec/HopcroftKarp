using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace HopcroftKarp.UnitTests
{
    public class TakeTests
    {
        [Test]
        public void TestTakeExtension()
        {
            var x = new List<int> { 1, 2, 3, 4 };
            var y = x.Take(2);
            Assert.AreEqual(x, new List<int> { 3, 4 });
            Assert.AreEqual(y, new List<int> { 1, 2 });
        }

        [Test]
        public void TestTakeExtensionOnSubSizeList()
        {
            var x = new List<int> { 1, 2 };
            var y = x.Take(3);
            Assert.AreEqual(x, new List<int> {  });
            Assert.AreEqual(y, new List<int> { 1, 2 });
        }
    }
}