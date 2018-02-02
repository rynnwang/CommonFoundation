using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beyova.ServiceUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var x = new List<BaseObject>();
            var y = x as IEnumerable<ISimpleBaseObject>;

            Assert.IsNotNull(y);
        }
    }
}
