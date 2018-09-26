using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beyova.Common.UnitTest
{
    [TestClass]
    public class RawHttpRequestUnitTest
    {
        [TestMethod]
        public void TestRawHttpRequest()
        {
            string raw = @"
GET https://www.telerik.com/UpdateCheck.aspx?isBeta=False HTTP/1.1
User-Agent: Fiddler/4.6.0.2 (.NET 4.0.30319.42000; WinNT 10.0.10240.0; zh-CN; 4xAMD64)
Pragma: no-cache
Host: www.telerik.com
Accept-Language: zh-CN
Referer: http://fiddler2.com/client/4.6.0.2
Accept-Encoding: gzip, deflate
Connection: close
";

            var request = raw.ToHttpRawMessage().ToHttpWebRequest();
            var result = request.ReadResponseAsText();

            Assert.IsNotNull(result);
        }
    }
}