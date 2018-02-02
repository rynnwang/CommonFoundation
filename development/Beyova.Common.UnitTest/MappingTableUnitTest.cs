using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beyova.Common.UnitTest
{
    [TestClass]
    public class MappingTableUnitTest
    {


        [TestMethod]
        public void General()
        {
            MappingTable mt = new MappingTable();
            mt.Add("1", "v1");
            mt.Add("2", "v2");

            var json = mt.ToJson();


            json = @"{
  ""valueUnique"": false,


  ""items"": [
    {
      ""s"": ""1"",
      ""m"": ""v1""
    },
    {
      ""s"": ""2"",
      ""m"": ""v2""
    }
  ],
 ""caseSensitive"": false

}";
            var mt2 = json.TryConvertJsonToObject<MappingTable<string>>();
            Assert.IsNotNull(mt2);
        }
    }
}
