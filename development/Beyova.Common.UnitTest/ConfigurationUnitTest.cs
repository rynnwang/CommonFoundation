using System;
using Beyova.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beyova.Common.UnitTest
{
    [TestClass]
    public class ConfigurationUnitTest
    {
        [TestMethod]
        public void TestConfiguration()
        {
            var jsonConfigurationReader = new JsonConfigurationReader();

            Assert.IsNotNull(jsonConfigurationReader);
            Assert.IsTrue(jsonConfigurationReader.Count > 0);

            var configValue3 = Framework.GetConfiguration<DateTime?>("Config3");
            Assert.IsNotNull(configValue3);
        }
    }
}