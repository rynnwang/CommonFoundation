using System;
using System.IO;
using System.Linq;
using System.Text;
using Beyova.UnitTestKit.InternalDoor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using static Beyova.Gravity.GravityConstants;

namespace Beyova.Gravity.UnitTest
{
    [TestClass]
    public class EntryFileGenerator : UnitTestKit.BaseUnitTest
    {
        static GravityManagementServiceCore serviceCore = new GravityManagementServiceCore();

        [TestMethod]
        public void Generate()
        {

            //File.WriteAllBytes(@"D:\", serviceCore.GenerateEntryFileBytes(new Guid(), new Uri("http://localhost"),))
        }

    }
}
