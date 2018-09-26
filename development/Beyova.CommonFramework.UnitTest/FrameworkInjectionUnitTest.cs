using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>
///
/// </summary>
namespace Beyova.Common.UnitTest
{
    [TestClass]
    public class FrameworkInjectionUnitTest
    {
        [TestMethod]
        public void TestFrameworkInjection()
        {
            ContextHelper.ApiContext.CurrentCredential = new BaseCredential { Name = "test" };
            Framework.CurrentOperatorCredential.CheckNullObject(nameof(Framework.CurrentOperatorCredential));
            Assert.AreEqual("test", Framework.CurrentOperatorCredential.Name);
        }
    }
}