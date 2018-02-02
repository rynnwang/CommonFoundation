using System;
using System.Threading;
using Beyova.Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beyova.RedisCache.UnitTest
{
    [TestClass]
    public class RedisCacheUnitTest
    {
        [TestMethod]
        public void RedisCacheUnitTest_StringKey()
        {
            const string key1 = "abc";
            const string key2 = "bcd";
            RedisCacheOptions redisOptions = new RedisCacheOptions()
            {
                UseEntityFullName = false,
                ExpirationInSecond = 5,
                DatabaseIndex = 5,
                Endpoints = new System.Collections.Generic.List<UriEndpoint> {
                    new UriEndpoint{  Host= "cne1qapdredis1.nsmena.ng.0001.cnn1.cache.amazonaws.com.cn", Port=6379},
                    new UriEndpoint{  Host= "cne1qapdredis1-002.nsmena.0001.cnn1.cache.amazonaws.com.cn", Port=6379}
                }
            };
            var cacheContainer = new RedisCacheContainer<string, string>(redisOptions);


            cacheContainer.Update(key1, "1");
            var expected1 = cacheContainer.Get(key1);

            Assert.AreEqual("1", expected1);
            Thread.Sleep(5500);

            var expected2 = cacheContainer.Get(key1);
            Assert.IsNull(expected2);
        }

        [TestMethod]
        public void RedisCacheUnitTest_StringKey_AutoRetrieval()
        {
            const string key1 = "abcd";
            const string key2 = "cdef";
            const string retrievalValue = "retrieval";
            RedisCacheOptions redisOptions = new RedisCacheOptions()
            {
                UseEntityFullName = false,
                ExpirationInSecond = 5,
                DatabaseIndex = 5,
                Endpoints = new System.Collections.Generic.List<UriEndpoint> {
                    new UriEndpoint{  Host= "cne1qapdredis1.nsmena.ng.0001.cnn1.cache.amazonaws.com.cn", Port=6379},
                    new UriEndpoint{  Host= "cne1qapdredis1-002.nsmena.0001.cnn1.cache.amazonaws.com.cn", Port=6379}
                }
            };
            var cacheContainer = new RedisCacheContainer<string, string>(
                redisOptions,
                retrievalOptions: new CacheAutoRetrievalOptions<string, string>((k) => { return retrievalValue; }));


            cacheContainer.Update(key1, "1");
            var expected1 = cacheContainer.Get(key1);

            Assert.AreEqual("1", expected1);
            Thread.Sleep(5500);

            var expected2 = cacheContainer.Get(key1);
            Assert.AreEqual(retrievalValue, expected2);

            var expected3 = cacheContainer.Get(key2);
            Assert.AreEqual(retrievalValue, expected3);

            Thread.Sleep(5500);
            var expected4 = cacheContainer.Get(key2);
            Assert.AreEqual(retrievalValue, expected4);
        }
    }
}
