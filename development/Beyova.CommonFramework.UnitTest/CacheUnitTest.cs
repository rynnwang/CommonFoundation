using System;
using System.Threading;
using Beyova.Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beyova.Common.UnitTest
{
    [TestClass]
    public class CacheUnitTest
    {
        [TestMethod]
        public void TestCache()
        {
            var container = new Cache.MemoryCacheContainer<string, string>(
                new MemoryCacheContainerOptions<string> { Capacity = 2, Name = "capacity test", ExpirationInSecond = 1 },
                retrievalOptions: new CacheAutoRetrievalOptions<string, string>(x => { return x + this.CreateRandomHexString(5); })
                );
            var c1 = container.Get("1");

            var c2 = container.Get("2");
            var c11 = container.Get("1");
            Assert.AreEqual(c1, c11);

            var c3 = container.Get("3");
            var c12 = container.Get("1");
            Assert.AreNotEqual(c1, c12);

            Thread.Sleep(100);

            var c13 = container.Get("1");
            Assert.AreEqual(c12, c13);

            Thread.Sleep(1000);

            var c14 = container.Get("1");
            Assert.AreNotEqual(c14, c13);

            container = new Cache.MemoryCacheContainer<string, string>(
                new MemoryCacheContainerOptions<string> { Capacity = 2, Name = "Comparer test", ExpirationInSecond = 1, EqualityComparer = StringComparer.OrdinalIgnoreCase },
                retrievalOptions: new CacheAutoRetrievalOptions<string, string>(x => { return x + this.CreateRandomHexString(5); })
                );

            var a = container.Get("a");
            var A = container.Get("A");
            Assert.AreEqual(a, A);

            container = new Cache.MemoryCacheContainer<string, string>(
              new MemoryCacheContainerOptions<string> { Capacity = 2, Name = "Comparer test2", ExpirationInSecond = 1, EqualityComparer = StringComparer.Ordinal },
              retrievalOptions: new CacheAutoRetrievalOptions<string, string>(x => { return x + this.CreateRandomHexString(5); })
              );
            var b = container.Get("b");
            var B = container.Get("B");
            Assert.AreNotEqual(b, B);
        }
    }
}