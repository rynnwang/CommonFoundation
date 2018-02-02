using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beyova.Common.UnitTest
{
    [TestClass]
    public class CultureResourceUnitTest
    {
        [TestInitialize]
        public void Prepare()
        {
        }

        [TestMethod]
        public void TestCultureResource()
        {
            ContextHelper.ApiContext.CultureCode = "zh-CN";
            var test1_CN = Framework.GetResourceString("test1");

            Assert.IsNotNull(test1_CN);

            ContextHelper.ApiContext.CultureCode = "en-US";
            var test2_EN = Framework.GetResourceString("test2");

            Assert.IsNotNull(test2_EN);
        }
    }
}