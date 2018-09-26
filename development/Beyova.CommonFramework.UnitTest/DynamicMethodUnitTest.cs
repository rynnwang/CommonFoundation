using System.Collections.Generic;
using System.IO;
using System.Linq;
using Beyova.ProgrammingIntelligence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beyova.Common.UnitTest
{
    [TestClass]
    public class DynamicMethodUnitTest
    {
        [TestMethod]
        public void TestDynamicMethod()
        {

            var staticMethod = DynamicStaticMethod.FromMethodCode(@"System.IO.File.WriteAllText(@""D:\test.txt"", ""Test Content"");");
            staticMethod.Invoke();
        }
    }
}