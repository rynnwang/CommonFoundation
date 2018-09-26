using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beyova.Common.UnitTest
{
    [TestClass]
    public class ZipUnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            Dictionary<string, byte[]> filesToZip = new Dictionary<string, byte[]>();
            filesToZip.Add("a.txt", Encoding.UTF8.GetBytes("Hello zip!"));
            filesToZip.Add("foldera/a.txt", Encoding.UTF8.GetBytes("Hello zip in folder!"));

            filesToZip.ZipToPath("D:\\test.zip");
            Assert.IsNotNull(filesToZip);
        }
    }
}