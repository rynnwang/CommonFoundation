using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beyova.Gravity.UnitTest
{
    [TestClass]
    public class ManagementUnitTest
    {
        static GravityManagementServiceCore serviceCore = new GravityManagementServiceCore();

        static ProductInfo testProduct = new ProductInfo
        {
            Name = "beyova test"
        };

        [TestMethod]
        public void TestCreateProduct()
        {
            var testProductKey = serviceCore.CreateOrUpdateProduct(testProduct);

            Assert.IsNotNull(testProductKey);

            var productByQuery = serviceCore.QueryProductInfo(new ProductCriteria { Key = testProductKey }).FirstOrDefault();
            Assert.IsNotNull(productByQuery);
            Assert.IsFalse(string.IsNullOrWhiteSpace(productByQuery.Token));
            Assert.IsFalse(string.IsNullOrWhiteSpace(productByQuery.PublicKey));
            Assert.IsFalse(string.IsNullOrWhiteSpace(productByQuery.PrivateKey));
        }

        [TestMethod]
        public void TestQueryClient()
        {
            var clients = serviceCore.QueryProductClient(new ProductClientCriteria { });

            Assert.IsNotNull(clients);
        }
    }
}
