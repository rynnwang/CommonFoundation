using System;
using Beyova.Diagnostic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beyova.ApiTracking.SqlServer.UnitTest
{
    [TestClass]
    public class ApiMessageUnitTest : BaseUnitTest
    {
        private ApiMessage testApiMessage = new ApiMessage
        {
            Message = string.Format("TESTED message on {0}", DateTime.Now.ToFullDateTimeString()),
            Category = "TEST",
            ServerIdentifier = EnvironmentCore.MachineName,
            ServiceIdentifier = EnvironmentCore.ProductName
        };

        [TestMethod]
        public void TestSave()
        {
            sqlClient.LogApiMessage(testApiMessage);
        }

        [TestMethod]
        public void TestLoad()
        {
            var expectNone = sqlClient.QueryApiMessage(new ApiMessageCriteria { ServerIdentifier = EnvironmentCore.MachineName + "1" });
            Assert.IsNotNull(expectNone);
            Assert.IsTrue(expectNone.Count == 0);

            var expectMore = sqlClient.QueryApiMessage(new ApiMessageCriteria { ServerIdentifier = EnvironmentCore.MachineName });
            Assert.IsNotNull(expectMore);
            Assert.IsTrue(expectMore.Count > 0);
        }
    }
}
