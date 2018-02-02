using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Beyova.Http;

namespace Beyova.Common.UnitTest
{
    [TestClass]
    public class HttpUnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            string text = "http://xx.com";

            var result1 = text.EnsureUrlProtocol("https");
            Assert.AreEqual(result1, "https://xx.com");

            text = "yy.com";
            var result2 = text.EnsureUrlProtocol("http");
            Assert.AreEqual(result2, "http://yy.com");

            text = "yy.com/?returnUrl=http://baidu.com";
            var result3 = text.EnsureUrlProtocol("ws");
            Assert.AreEqual(result3, "ws://yy.com/?returnUrl=http://baidu.com");
        }
    }
}
