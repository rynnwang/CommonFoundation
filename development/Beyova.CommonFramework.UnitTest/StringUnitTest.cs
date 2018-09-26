using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beyova.Common.UnitTest
{
    [TestClass]
    public class StringUnitTest
    {
        [TestMethod]
        public void TestMethod()
        {
            Regex guidRegex = new Regex(@"^[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}", RegexOptions.Compiled );
            var input = "9E49363E-82D8-4b4b-9359-045F2D5066BB.large";
            var match = guidRegex.Match(input);
            if (match.Success)
            {
                var x = match.Value;
            }
        }

        [TestMethod]
        public void TestMethod1()
        {
            string json = "[{\"Item\":\"Administration\"},{\"Item\":\"CreateOrUpdateAdminUser\"},{\"Item\":\"CreateOrUpdateAdminPermission\"}]";
            var result = json.SqlJsonToSimpleList<string>("Item");

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestEnsureCultureCode()
        {
            string cultureCode = "zh-CN";

            Assert.AreEqual(cultureCode, cultureCode.EnsureCultureCode());

            cultureCode = "en";
            Assert.AreEqual(cultureCode, cultureCode.EnsureCultureCode());

            cultureCode = "zh-Hans";
            Assert.IsNull(cultureCode.EnsureCultureCode());

            cultureCode = "zh-CN;q";
            Assert.IsNull(cultureCode.EnsureCultureCode());
        }
    }
}