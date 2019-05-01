using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beyova.ApiTracking.SqlServer.UnitTest
{
    [TestClass]
    public class BaseUnitTest
    {
        protected ApiTrackingSqlClient sqlClient = new ApiTrackingSqlClient("Data Source=tcp:database.beyova.com;Initial Catalog=LogSystem;Integrated Security=False;User ID=logagent;Password=123456");
    }
}
