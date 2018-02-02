using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beyova.Common.UnitTest
{
    [TestClass]
    public class AuthenticationUnitTest
    {

        public void SSOProviderUnitTest()
        {
            var provider = new SSOAuthorizationPartner { CallbackUrl = "http://callback.com/", Name = "UnitTestProvider", OwnerKey = Guid.NewGuid(), Token = this.CreateRandomString(32) };



        }
    }
}
