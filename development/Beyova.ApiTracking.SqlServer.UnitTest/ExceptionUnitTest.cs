using System;
using Beyova.Diagnostic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beyova.ApiTracking.SqlServer.UnitTest
{
    [TestClass]
    public class ExceptionUnitTest : BaseUnitTest
    {
        [TestMethod]
        public void TestSave()
        {
            var data = Method1();

            this.sqlClient.LogException(data);
        }

        [TestMethod]
        public void TestQuery()
        {
            var expectNone = sqlClient.QueryExceptionInfo(new  ExceptionCriteria { ServerIdentifier = EnvironmentCore.MachineName + "1" });
            Assert.IsNotNull(expectNone);
            Assert.IsTrue(expectNone.Count == 0);

            var expectMore = sqlClient.QueryExceptionInfo(new  ExceptionCriteria { ServerIdentifier = EnvironmentCore.MachineName });
            Assert.IsNotNull(expectMore);
            Assert.IsTrue(expectMore.Count > 0);

            Assert.IsNotNull(expectMore[0].Key);
            var getOne = sqlClient.QueryExceptionInfo(new ExceptionCriteria { Key = expectMore[0].Key }); ;
            Assert.IsNotNull(getOne);
        }

        private ExceptionInfo Method1()
        {
            try
            {
                Method2();
            }
            catch (Exception ex)
            {
                return ex.Handle().ToExceptionInfo();
            }

            return null;
        }

        private void Method2()
        {
            try
            {
                Method3();
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        private void Method3()
        {
            try
            {
                string x = null;
                x.CheckEmptyString(nameof(x));
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }
    }
}
