using Beyova.CodingBot.SqlServer;
using Beyova.CodingBot.VisualStudio;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beyova.Common.UnitTest
{
    [TestClass]
    public class ApiTrackingRestClientUnitTest
    {
        [TestMethod]
        public void MergeSql()
        {
            var dbRoot = DevelopmentEnvironment.GetCurrentProjectRootDirectory().Parent.GetSubDirectory("Beyova.ApiTracking.SqlServer").GetSubDirectory("Database");

            SqlPublishHelper.MergeSqlPublishScripts(dbRoot, dbRoot.GetSubDirectory("Setup"), "Table", null, null, "StoredProcedure");
        }

        [TestMethod]
        public void TestApiTrackingRestClient()
        {
            //var y = JToken.Parse("2015-11-11").Value<bool>();

            //var jtoken = JToken.Parse("{\"A\":{\"AA\":{}, \"BB\":[{\"C\":\"\"}]}}");
            //var x = jtoken.Values("A");
            //var aa = x.Values("AA");
            //var bb = x.Values("BB");

            //Assert.IsNotNull(aa);
            //Assert.IsNotNull(bb);

            //var client = new ApiTrackingRestClient("http://www.host.com/", "86300a43-5e5c-4062-8e40-7fd4e9fc6cdf");

            //client.LogApiEventAsync(new ApiEventLog
            //{
            //    ApiFullName = "test",
            //    RawUrl = "http://text.com/123/123/"
            //});
            //Assert.IsNotNull(client);
        }
    }
}