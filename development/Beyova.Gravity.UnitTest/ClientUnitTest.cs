using System;
using System.Linq;
using Beyova.UnitTestKit.InternalDoor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using static Beyova.Gravity.GravityConstants;

namespace Beyova.Gravity.UnitTest
{
    [TestClass]
    public class ClientUnitTest : UnitTestKit.BaseUnitTest
    {
        protected static GravityServiceCore serviceCore = new GravityServiceCore();

        protected static Guid? productKey = null;

        const string configurationName = "TestDefault";

        [TestInitialize]
        public void Prepare()
        {
            GravityManagementServiceCore mgmt = new GravityManagementServiceCore();
            productKey = mgmt.QueryProductInfo(new ProductCriteria { }).SafeFirstOrDefault()?.Key;

            var requestKey = mgmt.RequestCommand(new GravityCommandRequest { ProductKey = productKey, Action = BuiltInAction.UpdateConfiguration });

            this.SetContextUserInfo(Guid.NewGuid());
            mgmt.CreateOrUpdateRemoteConfigurationObject(new RemoteConfigurationObject { Name = configurationName, Configuration = null, OwnerKey = productKey });
        }

        [TestMethod]
        public void UpdateConfiguration()
        {
            productKey.CheckNullObject(nameof(productKey));

            var clientKey = serviceCore.SaveHeartbeatInfo(productKey, GravityKit.GetHeartbeat());
            var pendingCommands = serviceCore.GetPendingCommandRequest(clientKey);

            Assert.IsNotNull(pendingCommands);
            Assert.IsTrue(pendingCommands.Any());

            var configurationResult = serviceCore.QueryRemoteConfigurationObject(new RemoteConfigurationCriteria { OwnerKey = productKey, Name = configurationName }).FirstOrDefault();

            var testResult = new GravityCommandResult
            {
                ClientKey = clientKey,
                RequestKey = pendingCommands.First().Key,
                Content = JToken.FromObject(new RemoteConfigurationReceipt { Name = configurationResult.Name, Stamp = DateTime.UtcNow, SnapshotKey = configurationResult.SnapshotKey })
            };

            serviceCore.CommitCommandResult(testResult);
        }
    }
}
