using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beyova.Common.UnitTest
{
    [TestClass]
    public class UriUnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            string constUri = "http://beyova.com/";
            string constUriWithPath = "http://beyova.com/test";

            UriEndpoint endpoint = new UriEndpoint
            {
                Protocol = "http",
                Host = "beyova.com",
                Path = null
            };

            Assert.AreEqual(constUri, endpoint.ToString());


            UriEndpoint endpoint2 = new Uri(constUri);
            Assert.AreEqual(endpoint, endpoint2);


            UriEndpoint endpoint3 = new UriEndpoint
            {
                Protocol = "http",
                Host = "beyova.com",
                Path = "test"
            };

            Assert.AreEqual(constUriWithPath, endpoint3.ToString());

            UriEndpoint endpoint4 = new Uri(constUriWithPath);
            Assert.AreEqual(endpoint3, endpoint4);
        }
    }
}