//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using Newtonsoft.Json;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace Beyova.Common.UnitTest
//{
//    [TestClass]
//    public class DynamicCompileUnitTest
//    {
//        private const string coreCode = @"
//    public class Test
//    {
//        public int Run(int x)
//        {
//            return x + JsonConvert.DeserializeObject<int>(""1"");
//        }
//    }
//";

//        private const string fullCode = @"using System;
//using Newtonsoft.Json;

//namespace Beyova.Test{
//" + coreCode + @"
//}
//";

//        [TestMethod]
//        public void TestTempAssemblyProvider()
//        {
            
//            TempAssemblyProvider assemblyProvider = new TempAssemblyProvider();
//            var tempAssembly = assemblyProvider.CreateTempAssembly(fullCode.AsArray());

//            var obj = tempAssembly.CreateInstance("Test");
//            var method = obj.GetType().GetMethod("Run");
//            var result = (int)method.Invoke(obj, (2 as object).AsArray());

//            Assert.AreEqual(result, 3);
//        }

//        [TestMethod]
//        public void TestCompile()
//        {
//            var sandbox = new RuntimeAssemblySandbox("sandbox");
//            var assemblyName = sandbox.CreateTempAssembly(fullCode.AsArray());
//            var result = sandbox.InvokeRuntimeAssembly(assemblyName, "Test", "Run", (2 as object).AsArray());

//            Assert.IsNotNull(result);
//            Assert.AreEqual(result.GetObject(), 3);
//        }

//        [TestMethod]
//        public void SandboxMarshalInvokeResultUnitTest()
//        {
//            SandboxMarshalInvokeResult result = new SandboxMarshalInvokeResult();
//            Dictionary<string, byte[]> dic = new Dictionary<string, byte[]>();

//            dic.Add("Beyova.Common", File.ReadAllBytes(typeof(Framework).Assembly.Location));
//            result.SetValue(dic);

//            var turnedDic = BaseSandbox.WrapToCurrentAppDomain(result).GetObject() as Dictionary<string, byte[]>;
//            Assert.IsNotNull(turnedDic);
//            Assert.IsTrue(turnedDic.Any());
//        }
//    }
//}