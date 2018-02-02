using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beyova.Common.UnitTest
{
    [TestClass]
    public class DateUnitTest
    {
        [TestMethod]
        public void TestDateTimeKind()
        {
            Assert.AreEqual(1, new Date(new DateTime(1986, 12, 1, 12, 12, 12, DateTimeKind.Unspecified)).Day);
            Assert.AreEqual(1, new Date(new DateTime(1986, 12, 1, 16, 0, 0, DateTimeKind.Utc)).Day);

            Assert.AreEqual(7, new Date(1986, 12, 1) - new Date(1986, 11, 24));
            Assert.AreEqual(new Date(1986, 12, 1), new Date(new DateTime(1986, 12, 1, 12, 12, 12, DateTimeKind.Unspecified)));
        }

        [TestMethod]
        public void TestMinusOperator()
        {
            Assert.AreEqual(-365, new Date(1985, 11, 24) - new Date(1986, 11, 24));
            Assert.AreEqual(0, new Date(1985, 11, 24) - new Date(1985, 11, 24));
        }

        [TestMethod]
        public void TestAddOperator()
        {
            Assert.AreEqual(new Date(1986, 11, 25), new Date(1985, 11, 24) + 366);
        }
    }
}